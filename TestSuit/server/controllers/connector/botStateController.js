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
var ResponseTypes = require('../../../types/responseTypes');
var responseTypes_1 = require('../../../types/responseTypes');
var jsonBodyParser_1 = require('../../jsonBodyParser');
var BotStateController = (function () {
    function BotStateController() {
        var _this = this;
        this.botDataStore = {};
        // Get USER Data
        this.getUserData = function (req, res, next) {
            try {
                var botData = _this.getBotData(req.params.channelId, req.params.conversationId, req.params.userId);
                res.send(HttpStatus.OK, botData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // Get Conversation Data
        this.getConversationData = function (req, res, next) {
            try {
                var botData = _this.getBotData(req.params.channelId, req.params.conversationId, req.params.userId);
                res.send(HttpStatus.OK, botData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // Get PrivateConversation Data
        this.getPrivateConversationData = function (req, res, next) {
            try {
                var botData = _this.getBotData(req.params.channelId, req.params.conversationId, req.params.userId);
                res.send(HttpStatus.OK, botData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // Set User Data
        this.setUserData = function (req, res, next) {
            try {
                var newBotData = _this.setBotData(req.params.channelId, req.params.conversationId, req.params.userId, req.body);
                res.send(HttpStatus.OK, newBotData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // set conversation data
        this.setConversationData = function (req, res, next) {
            try {
                var newBotData = _this.setBotData(req.params.channelId, req.params.conversationId, req.params.userId, req.body);
                res.send(HttpStatus.OK, newBotData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // set private conversation data
        this.setPrivateConversationData = function (req, res, next) {
            try {
                var newBotData = _this.setBotData(req.params.channelId, req.params.conversationId, req.params.userId, req.body);
                res.send(HttpStatus.OK, newBotData);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
        // delete state for user
        this.deleteStateForUser = function (req, res, next) {
            try {
                var keys = Object.keys(_this.botDataStore);
                var userPostfix = "!" + req.params.userId;
                for (var i = 0; i < keys.length; i++) {
                    var key = keys[i];
                    if (key.endsWith(userPostfix)) {
                        delete _this.botDataStore[key];
                    }
                }
                res.send(HttpStatus.OK);
                res.end();
            }
            catch (err) {
                ResponseTypes.sendErrorResponse(req, res, next, err);
            }
        };
    }
    BotStateController.prototype.botDataKey = function (channelId, conversationId, userId) {
        return (channelId || '*') + "!" + (conversationId || '*') + "!" + (userId || '*');
    };
    BotStateController.prototype.getBotData = function (channelId, conversationId, userId) {
        var key = this.botDataKey(channelId, conversationId, userId);
        return this.botDataStore[key] || {
            data: null, eTag: 'empty'
        };
    };
    BotStateController.prototype.setBotData = function (channelId, conversationId, userId, incomingData) {
        var key = this.botDataKey(channelId, conversationId, userId);
        var oldData = this.botDataStore[key];
        if ((oldData && incomingData.eTag != "*") && oldData.eTag != incomingData.eTag) {
            throw ResponseTypes.createAPIException(HttpStatus.PRECONDITION_FAILED, responseTypes_1.ErrorCodes.BadArgument, "The data is changed");
        }
        var newData = {};
        newData.eTag = new Date().getTime().toString();
        newData.data = incomingData.data;
        this.botDataStore[key] = newData;
        return newData;
    };
    BotStateController.registerRoutes = function (server, auth) {
        var controller = new BotStateController();
        server.router.get('/v3/botstate/:channelId/users/:userId', auth.verifyBotFramework, controller.getUserData);
        server.router.get('/v3/botstate/:channelId/conversations/:conversationId', auth.verifyBotFramework, controller.getConversationData);
        server.router.get('/v3/botstate/:channelId/conversations/:conversationId/users/:userId', auth.verifyBotFramework, controller.getPrivateConversationData);
        server.router.post('/v3/botstate/:channelId/users/:userId', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [controller.setUserData]);
        server.router.post('/v3/botstate/:channelId/conversations/:conversationId', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [controller.setConversationData]);
        server.router.post('/v3/botstate/:channelId/conversations/:conversationId/users/:userId', [auth.verifyBotFramework], jsonBodyParser_1.jsonBodyParser(), [controller.setPrivateConversationData]);
        server.router.del('/v3/botstate/:channelId/users/:userId', auth.verifyBotFramework, controller.deleteStateForUser);
    };
    return BotStateController;
}());
exports.BotStateController = BotStateController;
//# sourceMappingURL=botStateController.js.map