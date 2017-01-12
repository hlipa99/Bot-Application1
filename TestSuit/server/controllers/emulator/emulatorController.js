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
var settings_1 = require('../../settings');
var emulator_1 = require('../../emulator');
var jsonBodyParser_1 = require('../../jsonBodyParser');
var ResponseTypes = require('../../../types/responseTypes');
var responseTypes_1 = require('../../../types/responseTypes');
function getConversation(conversationId) {
    var settings = settings_1.getSettings();
    var activeBot = settings.getActiveBot();
    if (!activeBot) {
        throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "bot not found");
    }
    var conversation = emulator_1.emulator.conversations.conversationById(activeBot.botId, conversationId);
    if (!conversation) {
        throw ResponseTypes.createAPIException(HttpStatus.NOT_FOUND, responseTypes_1.ErrorCodes.BadArgument, "conversation not found");
    }
    return conversation;
}
var EmulatorController = (function () {
    function EmulatorController() {
    }
    EmulatorController.registerRoutes = function (server) {
        server.router.get('/emulator/:conversationId/users', this.getUsers);
        server.router.post('/emulator/:conversationId/users', jsonBodyParser_1.jsonBodyParser(), this.addUsers);
        server.router.del('/emulator/:conversationId/users', this.removeUsers);
        server.router.post('/emulator/:conversationId/contacts', this.contactAdded);
        server.router.del('/emulator/:conversationId/contacts', this.contactRemoved);
        server.router.post('/emulator/:conversationId/typing', this.typing);
        server.router.post('/emulator/:conversationId/ping', this.ping);
        server.router.del('/emulator/:conversationId/userdata', this.deleteUserData);
    };
    EmulatorController.getUsers = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            res.json(HttpStatus.OK, conversation.members);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.addUsers = function (req, res, next) {
        try {
            var conversation_1 = getConversation(req.params.conversationId);
            var members = req.body;
            members.forEach(function (member) {
                conversation_1.addMember(member.id, member.name);
            });
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.removeUsers = function (req, res, next) {
        try {
            var conversation_2 = getConversation(req.params.conversationId);
            var members = req.body;
            if (!members) {
                var settings_2 = settings_1.getSettings();
                members = conversation_2.members.slice();
                members = members.filter(function (member) { return member.id != settings_2.users.currentUserId && member.id != conversation_2.botId; });
                members = members.slice(0);
            }
            members.forEach(function (member) {
                conversation_2.removeMember(member.id);
            });
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.contactAdded = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            conversation.sendContactAdded();
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.contactRemoved = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            conversation.sendContactRemoved();
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.typing = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            conversation.sendTyping();
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.ping = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            conversation.sendPing();
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    EmulatorController.deleteUserData = function (req, res, next) {
        try {
            var conversation = getConversation(req.params.conversationId);
            conversation.sendDeleteUserData();
            res.send(HttpStatus.OK);
            res.end();
        }
        catch (err) {
            ResponseTypes.sendErrorResponse(req, res, next, err);
        }
    };
    return EmulatorController;
}());
exports.EmulatorController = EmulatorController;
//# sourceMappingURL=emulatorController.js.map