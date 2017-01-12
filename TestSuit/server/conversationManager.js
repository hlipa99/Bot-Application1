//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Bot Framework: http://botframework.com
//
// Bot Framework Emulator Github:
// https://github.com/Microsoft/BotFramwork-Emulator
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
"use strict";
var request = require('request');
var utils_1 = require('../utils');
var settings_1 = require('./settings');
//import * as HttpStatus from "http-status-codes";
var ResponseTypes = require('../types/responseTypes');
var responseTypes_1 = require('../types/responseTypes');
var emulator_1 = require('./emulator');
var log = require('./log');
var utils = require('../utils');
var serverSettingsTypes_1 = require('../types/serverSettingsTypes');
/**
 * Stores and propagates conversation messages.
 */
var Conversation = (function () {
    function Conversation(botId, conversationId, user) {
        // the list of activities in this conversation
        this.activities = [];
        this.members = [];
        this.botId = botId;
        this.conversationId = conversationId;
        this.members.push({ id: botId, name: "Bot" });
        this.members.push({ id: user.id, name: user.name });
    }
    Conversation.prototype.getCurrentUser = function () {
        var users = settings_1.getSettings().users;
        var currentUser = users.usersById[users.currentUserId];
        // TODO: This is a band-aid until state system cleanup
        if (!currentUser) {
            currentUser = serverSettingsTypes_1.usersDefault.usersById['default-user'];
            settings_1.dispatch({
                type: 'Users_SetCurrentUser',
                state: {
                    user: currentUser
                }
            });
        }
        return currentUser;
    };
    Conversation.prototype.postage = function (recipientId, activity) {
        activity.id = activity.id || utils_1.uniqueId();
        activity.channelId = 'emulator';
        activity.timestamp = (new Date()).toISOString();
        activity.recipient = { id: recipientId };
        activity.conversation = { id: this.conversationId };
    };
    /**
     * Sends the activity to the conversation's bot.
     */
    Conversation.prototype.postActivityToBot = function (activity, recordInConversation, cb) {
        var _this = this;
        // Do not make a shallow copy here before modifying
        this.postage(this.botId, activity);
        activity.serviceUrl = emulator_1.emulator.framework.serviceUrl;
        activity.from = this.getCurrentUser();
        var bot = settings_1.getSettings().botById(this.botId);
        if (bot) {
            var options = { url: bot.botUrl, method: "POST", json: activity };
            var responseCallback = function (err, resp, body) {
                var messageActivity = activity;
                var text = messageActivity.text || '';
                if (text && text.length > 50)
                    text = text.substring(0, 50);
                if (err) {
                    log.error('->', log.makeInspectorLink("POST", activity), err.message);
                }
                else if (resp) {
                    if (!/^2\d\d$/.test("" + resp.statusCode)) {
                        log.error('->', log.makeInspectorLink("POST", activity), log.makeInspectorLink("" + resp.statusCode, body, "(" + resp.statusMessage + ")"), "[" + activity.type + "]", text);
                        if (Number(resp.statusCode) == 401 || Number(resp.statusCode) == 402) {
                            log.error("Error: The bot's MSA appId or passsword is incorrect.");
                            log.error(log.botCredsConfigurationLink('Click here'), "to edit your bot's MSA info.");
                        }
                        cb(err, resp ? resp.statusCode : undefined);
                    }
                    else {
                        log.info('->', log.makeInspectorLink("POST", activity), log.makeInspectorLink("" + resp.statusCode, body, "(" + resp.statusMessage + ")"), "[" + activity.type + "]", text);
                        if (recordInConversation) {
                            _this.activities.push(Object.assign({}, activity));
                        }
                        cb(null, resp.statusCode, activity.id);
                    }
                }
            };
            if (!utils.isLocalhostUrl(bot.botUrl) && utils.isLocalhostUrl(emulator_1.emulator.framework.serviceUrl)) {
                log.error('Error: The bot is remote, but the callback URL is localhost. Without tunneling software you will not receive replies.');
                log.error("Fix it:", log.ngrokConfigurationLink('Configure ngrok'));
                log.error('Learn more:', log.makeLinkMessage('Connecting to bots hosted remotely', 'https://github.com/Microsoft/BotFramework-Emulator/wiki/Getting-Started#connect-to-a-bot-hosted-remotely'));
            }
            if (bot.msaAppId && bot.msaPassword) {
                this.authenticatedRequest(options, responseCallback);
            }
            else {
                request(options, responseCallback);
            }
        }
        else {
            cb("bot not found");
        }
    };
    Conversation.prototype.sendConversationUpdate = function (membersAdded, membersRemoved) {
        var activity = {
            type: 'conversationUpdate',
            membersAdded: membersAdded,
            membersRemoved: membersRemoved
        };
        this.postActivityToBot(activity, false, function () { });
    };
    /**
     * Queues activity for delivery to user.
     */
    Conversation.prototype.postActivityToUser = function (activity) {
        var settings = settings_1.getSettings();
        // Make a shallow copy before modifying & queuing
        activity = Object.assign({}, activity);
        this.postage(settings.users.currentUserId, activity);
        var botId = activity.from.id;
        if (!activity.from.name) {
            activity.from.name = "Bot";
        }
        this.activities.push(activity);
        return ResponseTypes.createResourceResponse(activity.id);
    };
    // updateActivity with replacement
    Conversation.prototype.updateActivity = function (updatedActivity) {
        // if we found the activity to reply to
        var oldActivity = this.activities.find(function (val) { return val.id == updatedActivity.id; });
        if (oldActivity) {
            Object.assign(oldActivity, updatedActivity);
            return ResponseTypes.createResourceResponse(updatedActivity.id);
        }
        throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "not a known activity id");
    };
    Conversation.prototype.deleteActivity = function (id) {
        // if we found the activity to reply to
        var index = this.activities.findIndex(function (val) { return val.id == id; });
        if (index >= 0) {
            this.activities.splice(index, 1);
            return;
        }
        throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "The activity id was not found");
    };
    // add member
    Conversation.prototype.addMember = function (id, name) {
        name = name || "user-" + utils_1.uniqueId(4);
        id = id || utils_1.uniqueId();
        var user = { name: name, id: id };
        this.members.push(user);
        this.sendConversationUpdate([user], undefined);
        return user;
    };
    Conversation.prototype.removeMember = function (id) {
        var index = this.members.findIndex(function (val) { return val.id == id; });
        if (index >= 0) {
            this.members.splice(index, 1);
        }
        this.sendConversationUpdate(undefined, [{ id: id, name: undefined }]);
    };
    Conversation.prototype.sendContactAdded = function () {
        var activity = {
            type: 'contactRelationUpdate',
            action: 'add'
        };
        this.postActivityToBot(activity, false, function () { });
    };
    Conversation.prototype.sendContactRemoved = function () {
        var activity = {
            type: 'contactRelationUpdate',
            action: 'remove'
        };
        this.postActivityToBot(activity, false, function () { });
    };
    Conversation.prototype.sendTyping = function () {
        var activity = {
            type: 'typing'
        };
        this.postActivityToBot(activity, false, function () { });
    };
    Conversation.prototype.sendPing = function () {
        var activity = {
            type: 'ping'
        };
        this.postActivityToBot(activity, false, function () { });
    };
    Conversation.prototype.sendDeleteUserData = function () {
        var activity = {
            type: 'deleteUserData'
        };
        this.postActivityToBot(activity, false, function () { });
    };
    /**
     * Returns activities since the watermark.
     */
    Conversation.prototype.getActivitiesSince = function (watermark) {
        return this.activities.slice(watermark);
    };
    Conversation.prototype.authenticatedRequest = function (options, callback, refresh) {
        var _this = this;
        if (refresh === void 0) { refresh = false; }
        if (refresh) {
            this.accessToken = null;
        }
        this.addAccessToken(options, function (err) {
            if (!err) {
                request(options, function (err, response, body) {
                    if (!err) {
                        switch (response.statusCode) {
                            case HttpStatus.UNAUTHORIZED:
                            case HttpStatus.FORBIDDEN:
                                if (!refresh) {
                                    _this.authenticatedRequest(options, callback, true);
                                }
                                else {
                                    callback(null, response, body);
                                }
                                break;
                            default:
                                if (response.statusCode < 400) {
                                    callback(null, response, body);
                                }
                                else {
                                    var txt = "Request to '" + options.url + "' failed: [" + response.statusCode + "] " + response.statusMessage;
                                    callback(new Error(txt), response, null);
                                }
                                break;
                        }
                    }
                    else {
                        callback(err, null, null);
                    }
                });
            }
            else {
                callback(err, null, null);
            }
        });
    };
    Conversation.prototype.getAccessToken = function (cb) {
        var _this = this;
        if (!this.accessToken || new Date().getTime() >= this.accessTokenExpires) {
            var bot = settings_1.getSettings().botById(this.botId);
            // Refresh access token
            var opt = {
                method: 'POST',
                url: settings_1.v30AuthenticationSettings.tokenEndpoint,
                form: {
                    grant_type: 'client_credentials',
                    client_id: bot.msaAppId,
                    client_secret: bot.msaPassword,
                    scope: settings_1.v30AuthenticationSettings.tokenScope
                }
            };
            request(opt, function (err, response, body) {
                if (!err) {
                    if (body && response.statusCode < 300) {
                        // Subtract 5 minutes from expires_in so they'll we'll get a
                        // new token before it expires.
                        var oauthResponse = JSON.parse(body);
                        _this.accessToken = oauthResponse.access_token;
                        _this.accessTokenExpires = new Date().getTime() + ((oauthResponse.expires_in - 300) * 1000);
                        cb(null, _this.accessToken);
                    }
                    else {
                        cb(new Error('Refresh access token failed with status code: ' + response.statusCode), null);
                    }
                }
                else {
                    cb(err, null);
                }
            });
        }
        else {
            cb(null, this.accessToken);
        }
    };
    Conversation.prototype.addAccessToken = function (options, cb) {
        var bot = settings_1.getSettings().botById(this.botId);
        if (bot.msaAppId && bot.msaPassword) {
            this.getAccessToken(function (err, token) {
                if (!err && token) {
                    options.headers = {
                        'Authorization': 'Bearer ' + token
                    };
                    cb(null);
                }
                else {
                    cb(err);
                }
            });
        }
        else {
            cb(null);
        }
    };
    return Conversation;
}());
exports.Conversation = Conversation;
/**
 * A set of conversations with a bot.
 */
var ConversationSet = (function () {
    function ConversationSet(botId) {
        this.conversations = [];
        this.botId = botId;
    }
    ConversationSet.prototype.newConversation = function (user, conversationId) {
        var conversation = new Conversation(this.botId, conversationId || utils_1.uniqueId(), user);
        this.conversations.push(conversation);
        return conversation;
    };
    ConversationSet.prototype.conversationById = function (conversationId) {
        return this.conversations.find(function (value) { return value.conversationId === conversationId; });
    };
    return ConversationSet;
}());
/**
 * Container for conversations.
 */
var ConversationManager = (function () {
    function ConversationManager() {
        var _this = this;
        this.conversationSets = [];
        settings_1.addSettingsListener(function (settings) {
            _this.configure(settings);
        });
        this.configure(settings_1.getSettings());
    }
    /**
     * Applies configuration changes.
     */
    ConversationManager.prototype.configure = function (settings) {
        // Remove conversations that reference nonexistent bots.
        var deadBotIds = this.conversationSets.filter(function (set) { return !settings.bots.find(function (bot) { return bot.botId === set.botId; }); }).map(function (conversation) { return conversation.botId; });
        this.conversationSets = this.conversationSets.filter(function (set) { return !deadBotIds.find(function (botId) { return set.botId === botId; }); });
    };
    /**
     * Creates a new conversation.
     */
    ConversationManager.prototype.newConversation = function (botId, user, conversationId) {
        var conversationSet = this.conversationSets.find(function (value) { return value.botId === botId; });
        if (!conversationSet) {
            conversationSet = new ConversationSet(botId);
            this.conversationSets.push(conversationSet);
        }
        var conversation = conversationSet.newConversation(user, conversationId);
        return conversation;
    };
    /**
     * Gets the existing conversation, or returns undefined.
     */
    ConversationManager.prototype.conversationById = function (botId, conversationId) {
        var set = this.conversationSets.find(function (set) { return set.botId === botId; });
        if (set) {
            return set.conversationById(conversationId);
        }
    };
    return ConversationManager;
}());
exports.ConversationManager = ConversationManager;
//# sourceMappingURL=conversationManager.js.map