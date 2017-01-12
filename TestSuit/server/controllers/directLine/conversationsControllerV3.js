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
var HttpStatus = require("http-status-codes");
var emulator_1 = require('../../emulator');
var settings_1 = require('../../settings');
var attachmentsController_1 = require('../connector/attachmentsController');
var log = require('../../log');
var Fs = require('fs');
var Formidable = require('formidable');
var jsonBodyParser_1 = require('../../jsonBodyParser');
var serverSettingsTypes_1 = require('../../../types/serverSettingsTypes');
var ConversationsControllerV3 = (function () {
    function ConversationsControllerV3() {
    }
    ConversationsControllerV3.registerRoutes = function (server) {
        server.router.opts('/v3/directline', this.options);
        server.router.post('/v3/directline/conversations', jsonBodyParser_1.jsonBodyParser(), this.startConversation);
        server.router.get('/v3/directline/conversations/:conversationId', this.reconnectToConversation);
        server.router.get('/v3/directline/conversations/:conversationId/activities/', this.getActivities);
        server.router.post('/v3/directline/conversations/:conversationId/activities', jsonBodyParser_1.jsonBodyParser(), this.postActivity);
        server.router.post('/v3/directline/conversations/:conversationId/upload', this.upload);
        server.router.get('/v3/directline/conversations/:conversationId/stream', this.stream);
    };
    ConversationsControllerV3.options = function (req, res, next) {
        res.send(HttpStatus.OK);
        res.end();
    };
    ConversationsControllerV3.startConversation = function (req, res, next) {
        var activeBot = settings_1.getSettings().getActiveBot();
        if (activeBot) {
            var created = false;
            var auth = req.header('Authorization');
            var tokenMatch = /Bearer\s+(.+)/.exec(auth);
            var conversationId = tokenMatch[1];
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, conversationId);
            if (!conversation) {
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
                conversation = emulator_1.emulator.conversations.newConversation(activeBot.botId, currentUser, conversationId);
                conversation.sendConversationUpdate(conversation.members, undefined);
                created = true;
            }
            res.json(created ? HttpStatus.CREATED : HttpStatus.OK, {
                conversationId: conversation.conversationId,
                token: conversation.conversationId,
                expires_in: Number.MAX_VALUE,
                streamUrl: ''
            });
        }
        else {
            res.send(HttpStatus.NOT_FOUND, "no active bot");
            log.error("DirectLine: Cannot start conversation. No active bot");
        }
        res.end();
    };
    ConversationsControllerV3.reconnectToConversation = function (req, res, next) {
        var activeBot = settings_1.getSettings().getActiveBot();
        if (activeBot) {
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, req.params.conversationId);
            if (conversation) {
                res.json(HttpStatus.OK, {
                    conversationId: conversation.conversationId,
                    token: conversation.conversationId,
                    expires_in: Number.MAX_VALUE,
                    streamUrl: ''
                });
            }
            else {
                res.send(HttpStatus.NOT_FOUND, "conversation not found");
                log.error("DirectLine: Cannot post activity. Conversation not found");
            }
        }
        else {
            res.send(HttpStatus.NOT_FOUND, "no active bot");
            log.error("DirectLine: Cannot start conversation. No active bot");
        }
        res.end();
    };
    ConversationsControllerV3.getActivities = function (req, res, next) {
        var activeBot = settings_1.getSettings().getActiveBot();
        if (activeBot) {
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, req.params.conversationId);
            if (conversation) {
                var watermark = Number(req.params.watermark || 0) || 0;
                var activities = conversation.getActivitiesSince(req.params.watermark);
                res.json(HttpStatus.OK, {
                    activities: activities,
                    watermark: watermark + activities.length
                });
            }
            else {
                res.send(HttpStatus.NOT_FOUND, "conversation not found");
                log.error("DirectLine: Cannot get activities. Conversation not found");
            }
        }
        else {
            res.send(HttpStatus.NOT_FOUND, "no active bot");
            log.error("DirectLine: Cannot get activities. No active bot");
        }
        res.end();
    };
    ConversationsControllerV3.postActivity = function (req, res, next) {
        var activeBot = settings_1.getSettings().getActiveBot();
        if (activeBot) {
            var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, req.params.conversationId);
            if (conversation) {
                var activity = req.body;
                conversation.postActivityToBot(activity, true, function (err, statusCode, activityId) {
                    if (err || !/^2\d\d$/.test("" + statusCode)) {
                        res.send(statusCode || HttpStatus.INTERNAL_SERVER_ERROR);
                    }
                    else {
                        res.send(statusCode, { id: activityId });
                    }
                    res.end();
                });
            }
            else {
                res.send(HttpStatus.NOT_FOUND, "conversation not found");
                log.error("DirectLine: Cannot post activity. Conversation not found");
                res.end();
            }
        }
        else {
            res.send(HttpStatus.NOT_FOUND, "no active bot");
            log.error("DirectLine: Cannot post activity. No active bot");
            res.end();
        }
    };
    ConversationsControllerV3.upload = function (req, res, next) {
        var settings = settings_1.getSettings();
        var activeBot = settings.getActiveBot();
        var currentUser = settings.users.usersById[settings.users.currentUserId];
        if (activeBot) {
            var conversation_1 = emulator_1.emulator.conversations.conversationById(activeBot.botId, req.params.conversationId);
            if (conversation_1) {
                if (req.getContentType() !== 'multipart/form-data' ||
                    (req.getContentLength() === 0 && !req.isChunked())) {
                    return undefined;
                }
                var form = new Formidable.IncomingForm();
                form.multiples = true;
                form.keepExtensions = true;
                // TODO: Override form.onPart handler so it doesn't write temp files to disk.
                form.parse(req, function (err, fields, files) {
                    try {
                        var activity_1 = JSON.parse(Fs.readFileSync(files.activity.path, 'utf8'));
                        var uploads = files.file;
                        if (!Array.isArray(uploads))
                            uploads = [uploads];
                        if (uploads && uploads.length) {
                            activity_1.attachments = [];
                            uploads.forEach(function (upload) {
                                var name = upload.name || 'file.dat';
                                var type = upload.type;
                                var path = upload.path;
                                var buf = Fs.readFileSync(path);
                                var contentBase64 = buf.toString('base64');
                                var attachmentData = {
                                    type: type,
                                    name: name,
                                    originalBase64: contentBase64,
                                    thumbnailBase64: contentBase64
                                };
                                var attachmentId = attachmentsController_1.AttachmentsController.uploadAttachment(attachmentData);
                                var attachment = {
                                    name: name,
                                    contentType: type,
                                    contentUrl: emulator_1.emulator.framework.serviceUrl + "/v3/attachments/" + attachmentId + "/views/original"
                                };
                                activity_1.attachments.push(attachment);
                            });
                            conversation_1.postActivityToBot(activity_1, true, function (err, statusCode, activityId) {
                                if (err || !/^2\d\d$/.test("" + statusCode)) {
                                    res.send(statusCode || HttpStatus.INTERNAL_SERVER_ERROR);
                                }
                                else {
                                    res.send(statusCode, { id: activityId });
                                }
                                res.end();
                            });
                        }
                        else {
                            res.send(HttpStatus.BAD_REQUEST, "no file uploaded");
                            log.error("DirectLine: Cannot post activity. No file uploaded");
                            res.end();
                        }
                    }
                    catch (e) {
                        res.send(HttpStatus.INTERNAL_SERVER_ERROR, "error processing uploads");
                        log.error("DirectLine: Failed to post activity. No files uploaded");
                        res.end();
                    }
                });
            }
            else {
                res.send(HttpStatus.NOT_FOUND, "conversation not found");
                log.error("DirectLine: Cannot post activity. Conversation not found");
                res.end();
            }
        }
        else {
            res.send(HttpStatus.NOT_FOUND, "no active bot");
            log.error("DirectLine: Cannot post activity. No active bot");
            res.end();
        }
    };
    ConversationsControllerV3.stream = function (req, res, next) {
        res.send(HttpStatus.NOT_IMPLEMENTED);
        log.error("DirectLine: Cannot upgrade socket. Not implemented.");
        res.end();
    };
    return ConversationsControllerV3;
}());
exports.ConversationsControllerV3 = ConversationsControllerV3;
//# sourceMappingURL=conversationsControllerV3.js.map