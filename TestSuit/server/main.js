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
var Electron = require('electron');
var electron_1 = require('electron');
var settings_1 = require('./settings');
var url = require('url');
var path = require('path');
var log = require('./log');
var emulator_1 = require('./emulator');
var pjson = require('../../package.json');
process.on('uncaughtException', function (error) {
    console.error(error);
    log.error('[err-server]', error.message.toString(), JSON.stringify(error.stack));
});
var createMainWindow = function () {
    // TODO: Make a better/safer window state restoration module
    // (handles change in display dimensions, maximized state, etc)
    var safeLowerBound = function (val, lowerBound) {
        if (typeof (val) === 'number') {
            return Math.max(lowerBound, val);
        }
    };
    var settings = settings_1.getSettings();
    exports.mainWindow = new Electron.BrowserWindow({
        width: safeLowerBound(settings.windowState.width, 0),
        height: safeLowerBound(settings.windowState.height, 0),
        x: safeLowerBound(settings.windowState.left, 0),
        y: safeLowerBound(settings.windowState.top, 0)
    });
    exports.mainWindow.setTitle("Microsoft Bot Framework Emulator (v" + pjson.version + ")");
    //mainWindow.webContents.openDevTools();
    if (process.platform === 'darwin') {
        // Create the Application's main menu
        var template = [
            {
                label: "Bot Framework Emulator",
                submenu: [
                    { label: "About", click: function () { return emulator_1.Emulator.send('show-about'); } },
                    { type: "separator" },
                    { label: "Quit", accelerator: "Command+Q", click: function () { return Electron.app.quit(); } }
                ]
            }, {
                label: "Edit",
                submenu: [
                    { label: "Undo", accelerator: "CmdOrCtrl+Z", role: "undo" },
                    { label: "Redo", accelerator: "Shift+CmdOrCtrl+Z", role: "redo" },
                    { type: "separator" },
                    { label: "Cut", accelerator: "CmdOrCtrl+X", role: "cut" },
                    { label: "Copy", accelerator: "CmdOrCtrl+C", role: "copy" },
                    { label: "Paste", accelerator: "CmdOrCtrl+V", role: "paste" }
                ] }
        ];
        electron_1.Menu.setApplicationMenu(electron_1.Menu.buildFromTemplate(template));
    }
    else {
        electron_1.Menu.setApplicationMenu(null);
    }
    exports.mainWindow.on('resize', function () {
        var bounds = exports.mainWindow.getBounds();
        settings_1.dispatch({
            type: 'Window_RememberBounds',
            state: {
                width: bounds.width,
                height: bounds.height,
                left: bounds.x,
                top: bounds.y
            }
        });
    });
    exports.mainWindow.on('move', function () {
        var bounds = exports.mainWindow.getBounds();
        settings_1.dispatch({
            type: 'Window_RememberBounds',
            state: {
                width: bounds.width,
                height: bounds.height,
                left: bounds.x,
                top: bounds.y
            }
        });
    });
    exports.mainWindow.on('closed', function () {
        exports.mainWindow = null;
    });
    exports.mainWindow.webContents.once('did-finish-load', function () {
        var page = url.format({
            protocol: 'file',
            slashes: true,
            pathname: path.join(__dirname, '../client/index.html')
        });
        exports.mainWindow.loadURL(page);
    });
    var splash = url.format({
        protocol: 'file',
        slashes: true,
        pathname: path.join(__dirname, '../client/splash.html')
    });
    exports.mainWindow.loadURL(splash);
};
var shouldQuit = Electron.app.makeSingleInstance(function (commandLine, workingDirectory) {
    if (exports.mainWindow) {
        if (exports.mainWindow.isMinimized())
            exports.mainWindow.restore();
        exports.mainWindow.focus();
    }
});
if (shouldQuit) {
    Electron.app.quit();
}
else {
    emulator_1.Emulator.startup();
    Electron.app.on('ready', createMainWindow);
    Electron.app.on('window-all-closed', function () {
        if (process.platform !== 'darwin') {
            Electron.app.quit();
        }
    });
    Electron.app.on('activate', function () {
        if (exports.mainWindow === null) {
            createMainWindow();
        }
    });
}
// Do this last, otherwise startup bugs are harder to diagnose.
require('electron-debug')();
//# sourceMappingURL=main.js.map