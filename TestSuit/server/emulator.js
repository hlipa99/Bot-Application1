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
var botFrameworkService_1 = require('./botFrameworkService');
var conversationManager_1 = require('./conversationManager');
var Settings = require('./settings');
var Electron = require('electron');
var main_1 = require('./main');
/**
 * Top-level state container for the Node process.
 */
var Emulator = (function () {
    function Emulator() {
        var _this = this;
        this.framework = new botFrameworkService_1.BotFrameworkService();
        this.conversations = new conversationManager_1.ConversationManager();
        // When the client notifies us it has started up, send it the configuration.
        // Note: We're intentionally sending and ISettings here, not a Settings. This
        // is why we're getting the value from getStore().getState().
        Electron.ipcMain.on('clientStarted', function () {
            _this.mainWindow = main_1.mainWindow;
            Emulator.queuedMessages.forEach(function (msg) {
                Emulator.send.apply(Emulator, [msg.channel].concat(msg.args));
            });
            Emulator.queuedMessages = [];
            Emulator.send('serverSettings', Settings.getStore().getState());
        });
        Settings.addSettingsListener(function () {
            Emulator.send('serverSettings', Settings.getStore().getState());
        });
    }
    /**
     * Loads settings from disk and then creates the emulator.
     */
    Emulator.startup = function () {
        Settings.startup();
        exports.emulator = new Emulator();
        exports.emulator.framework.startup();
    };
    /**
     * Sends a command to the client.
     */
    Emulator.send = function (channel) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        if (main_1.mainWindow) {
            (_a = main_1.mainWindow.webContents).send.apply(_a, [channel].concat(args));
        }
        else {
            Emulator.queuedMessages.push({ channel: channel, args: args });
        }
        var _a;
    };
    Emulator.queuedMessages = [];
    return Emulator;
}());
exports.Emulator = Emulator;
//# sourceMappingURL=emulator.js.map