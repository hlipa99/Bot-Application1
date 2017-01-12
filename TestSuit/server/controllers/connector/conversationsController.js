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
var settings_1 = require('../../settings');
var emulator_1 = require('../../emulator');
var HttpStatus = require("http-status-codes");
var ResponseTypes = require('../../../types/responseTypes');
var responseTypes_1 = require('../../../types/responseTypes');
var attachmentsController_1 = require('./attachmentsController');
var log = require('../../log');
var log_1 = require('../../log');
var jsonBodyParser_1 = require('../../jsonBodyParser');
var ConversationsController = (function () {
    function ConversationsController() {
    }
    ConversationsController.registerRoutes = function (server, auth) {
        server.router.post('/v3/conversations', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [this.createConversation]);
        server.router.post('/v3/conversations/:conversationId/activities', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [this.sendToConversation]);
        server.router.post('/v3/conversations/:conversationId/activities/:activityId', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [this.replyToActivity]);
        server.router.put('/v3/conversations/:conversationId/activities/:activityId', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [this.updateActivity]);
        server.router.del('/v3/conversations/:conversationId/activities/:activityId', auth.verifyBotFramework, this.deleteActivity);
        server.router.get('/v3/conversations/:conversationId/members', auth.verifyBotFramework, this.getConversationMembers);
        server.router.get('/v3/conversations/:conversationId/activities/:activityId/members', auth.verifyBotFramework, this.getActivityMembers);
        server.router.post('/v3/conversations/:conversationId/attachments', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [this.uploadAttachment]);
    };
    // Create conversation API
    ConversationsController.createConversation = function (req, res, next) {
        var conversationParameters = req.body;
        try {
            console.log("framework: newConversation");
            var settings = settings_1.getSettings();
            // look up bot
            var activeBot = settings.getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            var users = settings.users;
            if (conversationParameters.members == null)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.MissingProperty, "members missing");
            if (conversationParameters.members.length != 1)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.BadSyntax, "Emulator only supports creating conversation with 1 user");
            if (conversationParameters.members[0].id !== settings.users.currentUserId)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.BadSyntax, "Emulator only supports creating conversation with the current user");
            if (conversationParameters.bot == null)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.MissingProperty, "missing Bot property");
            if (conversationParameters.bot.id != activeBot.botId)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.BadArgument, "conversationParameters.bot.id doesn't match security bot id");
            var newUsers = [];
            // merge users in
            for (var key in conversationParameters.members) {
                newUsers.push({
                    id: conversationParameters.members[key].id,
                    name: conversationParameters.members[key].name
                });
            }
            settings_1.getStore().dispatch({
                type: "Users_AddUsers",
                state: { users: newUsers }
            });
            var newConversation = void 0;
            if (conversationParameters.conversationId) {
                newConversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, conversationParameters.conversationId);
            }
            if (!newConversation) {
                newConversation = emulator_1.emulator.conversations.newConversation(activeBot.botId, users.usersById[conversationParameters.members[0].id], conversationParameters.conversationId);
            }
            var activityId = null;
            if (conversationParameters.activity != null) {
                // set routing information for new conversation
                conversationParameters.activity.conversation = { id: newConversation.conversationId };
                conversationParameters.activity.from = { id: activeBot.botId };
                conversationParameters.activity.recipient = { id: conversationParameters.members[0].id };
                var response_1 = newConversation.postActivityToUser(conversationParameters.activity);
                activityId = response_1.id;
            }
            var response = ResponseTypes.createConversationResponse(newConversation.conversationId, activityId);
            res.send(HttpStatus.OK, response);
            res.end();
            log.api('createConversation', req, res, conversationParameters, response, getActivityText(conversationParameters.activity));
            // Tell the client side to start a new conversation.
            emulator_1.Emulator.send('new-conversation', newConversation.conversationId);
        }
        catch (err) {
            var error = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api('createConversation', req, res, conversationParameters, error, getActivityText(conversationParameters.activity));
        }
    };
    // SendToConversation
    ConversationsController.sendToConversation = function (req, res, next) {
        var activity = req.body;
        try {
            var parms = req.params;
            console.log("framework: sendToConversation", JSON.stringify(activity));
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            activity.replyToId = req.params.activityId;
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            // post activity
            var response = conversation.postActivityToUser(activity);
            res.send(HttpStatus.OK, response);
            res.end();
            log.api("Send[" + activity.type + "]", req, res, activity, response, getActivityText(activity));
        }
        catch (err) {
            var error_1 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api("Send[" + activity.type + "]", req, res, activity, error_1, getActivityText(activity));
        }
    };
    // replyToActivity
    ConversationsController.replyToActivity = function (req, res, next) {
        var activity = req.body;
        try {
            var parms = req.params;
            console.log("framework: replyToActivity", JSON.stringify(activity));
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            activity.replyToId = req.params.activityId;
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            // if we found the activity to reply to
            //if (!conversation.activities.find((existingActivity, index, obj) => existingActivity.id == activity.replyToId))
            //    throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, ErrorCodes.BadArgument, "replyToId is not a known activity id");
            // post activity
            var response = conversation.postActivityToUser(activity);
            res.send(HttpStatus.OK, response);
            res.end();
            log.api("Reply[" + activity.type + "]", req, res, activity, response, getActivityText(activity));
        }
        catch (err) {
            var error_2 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api("Reply[" + activity.type + "]", req, res, activity, error_2, getActivityText(activity));
        }
    };
    // updateActivity
    ConversationsController.updateActivity = function (req, res, next) {
        var activity = req.body;
        try {
            var parms = req.params;
            console.log("framework: updateActivity", JSON.stringify(activity));
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            activity.replyToId = req.params.activityId;
            if (activity.id != parms.activityId)
                throw ResponseTypes.createAPIException(HttpStatus.BAD_REQUEST, responseTypes_1.ErrorCodes.BadArgument, "uri activity id does not match payload activity id");
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            // post activity
            var response = conversation.updateActivity(activity);
            res.send(HttpStatus.OK, response);
            res.end();
            log.api("Update[" + activity.id + "]", req, res, activity, response, getActivityText(activity));
        }
        catch (err) {
            var error_3 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api("Update[" + activity.id + "]", req, res, activity, error_3, getActivityText(activity));
        }
    };
    // deleteActivity
    ConversationsController.deleteActivity = function (req, res, next) {
        var parms = req.params;
        try {
            console.log("framework: deleteActivity", JSON.stringify(parms));
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            conversation.deleteActivity(parms.activityId);
            res.send(HttpStatus.OK);
            res.end();
            log.api("DeleteActivity(" + parms.activityId + ")", req, res);
        }
        catch (err) {
            var error_4 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api("DeleteActivity(" + parms.activityId + ")", req, res, null, error_4);
        }
    };
    // get members of a conversation
    ConversationsController.getConversationMembers = function (req, res, next) {
        console.log("framework: getConversationMembers");
        var parms = req.params;
        try {
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            res.send(HttpStatus.OK, conversation.members);
            res.end();
            log.api("GetConversationMembers(" + parms.conversationId + ")", req, res, null, conversation.members);
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api("GetConversationMembers(" + parms.conversationId + ")", req, res, null, log_1.error);
        }
    };
    // get members of an activity
    ConversationsController.getActivityMembers = function (req, res, next) {
        console.log("framework: getActivityMembers");
        var parms = req.params;
        try {
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            var activity = req.body;
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            res.send(HttpStatus.OK, conversation.members);
            res.end();
            log.api("GetActivityMembers(" + parms.activityId + ")", req, res, null, conversation.members);
        }
        catch (err) {
            var error_5 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.error("GetActivityMembers(" + parms.activityId + ")", req, res, null, error_5);
        }
    };
    // upload attachment
    ConversationsController.uploadAttachment = function (req, res, next) {
        console.log("framework: uploadAttachment");
        var attachmentData = req.body;
        try {
            // look up bot
            var activeBot = settings_1.getSettings().getActiveBot();
            if (!activeBot)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
            var parms = req.params;
            // look up conversation
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, parms.conversationId);
            if (!conversation)
                throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
            var resourceId = attachmentsController_1.AttachmentsController.uploadAttachment(attachmentData);
            var resourceResponse = { id: resourceId };
            res.send(HttpStatus.OK, resourceResponse);
            res.end();
            log.api('UploadAttachment()', req, res, attachmentData, resourceResponse, attachmentData.name);
        }
        catch (err) {
            var error_6 = ResponseTypes.sendErrorResponse(req, res, next, err);
            log.api('UploadAttachment()', req, res, attachmentData, error_6, attachmentData.name);
        }
    };
    return ConversationsController;
}());
exports.ConversationsController = ConversationsController;
function getActivityText(activity) {
    if (activity) {
        if (activity.attachments && activity.attachments.length > 0)
            return activity.attachments[0].contentType;
        else {
            if (activity.text.length > 50)
                return activity.text.substring(0, 50) + '...';
            return activity.text;
        }
    }
    return '';
}
//# sourceMappingURL=conversationsController.js.map