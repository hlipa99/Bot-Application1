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
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var botFrameworkAuthentication_1 = require('./botFrameworkAuthentication');
var conversationsController_1 = require('./controllers/connector/conversationsController');
var attachmentsController_1 = require('./controllers/connector/attachmentsController');
var botStateController_1 = require('./controllers/connector/botStateController');
var conversationsControllerV3_1 = require('./controllers/directLine/conversationsControllerV3');
var emulatorController_1 = require('./controllers/emulator/emulatorController');
var restServer_1 = require('./restServer');
var settings_1 = require('./settings');
var log = require('./log');
var ngrok = require('./ngrok');
var emulator_1 = require('./emulator');
/**
 * Communicates with the bot.
 */
var BotFrameworkService = (function (_super) {
    __extends(BotFrameworkService, _super);
    function BotFrameworkService() {
        var _this = this;
        _super.call(this, "Emulator");
        this.authentication = new botFrameworkAuthentication_1.BotFrameworkAuthentication();
        conversationsController_1.ConversationsController.registerRoutes(this, this.authentication);
        attachmentsController_1.AttachmentsController.registerRoutes(this);
        botStateController_1.BotStateController.registerRoutes(this, this.authentication);
        conversationsControllerV3_1.ConversationsControllerV3.registerRoutes(this);
        emulatorController_1.EmulatorController.registerRoutes(this);
        settings_1.addSettingsListener(function (settings) {
            _this.configure(settings);
        });
        this.router.on('listening', function () {
            _this.relaunchNgrok();
            emulator_1.Emulator.send('listening', { serviceUrl: _this.serviceUrl });
        });
    }
    Object.defineProperty(BotFrameworkService.prototype, "serviceUrl", {
        get: function () {
            return ngrok.running()
                ? this.ngrokServiceUrl || this._serviceUrl
                : this._serviceUrl;
        },
        enumerable: true,
        configurable: true
    });
    BotFrameworkService.prototype.startup = function () {
        this.restart();
    };
    BotFrameworkService.prototype.relaunchNgrok = function () {
        var _this = this;
        var router = this.router;
        if (!router)
            return;
        var address = router.address();
        if (!address)
            return;
        var port = address.port;
        if (!port)
            return;
        var settings = settings_1.getSettings();
        var prevNgrokPath = this.ngrokPath;
        this.ngrokPath = settings.framework.ngrokPath;
        var prevServiceUrl = this.serviceUrl;
        this._serviceUrl = "http://localhost:" + port;
        this.inspectUrl = null;
        this.ngrokServiceUrl = null;
        var startNgrok = function () {
            // if we have an ngrok path
            if (_this.ngrokPath) {
                // then make it so
                ngrok.connect({
                    port: port,
                    path: _this.ngrokPath
                }, function (err, url, inspectPort) {
                    if (err) {
                        log.warn("Failed to start ngrok: " + (err.message || err.msg));
                        log.error("Fix it:", log.ngrokConfigurationLink('Configure ngrok'));
                        log.error("Learn more:", log.makeLinkMessage('Network tunneling (ngrok)', 'https://github.com/Microsoft/BotFramework-Emulator/wiki/Tunneling-(ngrok)'));
                    }
                    else {
                        _this.inspectUrl = "http://localhost:" + inspectPort;
                        _this.ngrokServiceUrl = url;
                        log.debug("ngrok listening on " + url);
                        log.debug('ngrok traffic inspector:', log.makeLinkMessage(_this.inspectUrl, _this.inspectUrl));
                    }
                    // Sync settings to client
                    settings_1.getStore().dispatch({
                        type: 'Framework_Set',
                        state: {
                            ngrokPath: _this.ngrokPath
                        }
                    });
                });
            }
        };
        if (this.ngrokPath !== prevNgrokPath) {
            ngrok.kill(function (wasRunning) {
                if (wasRunning)
                    log.debug('ngrok stopped');
                startNgrok();
                return true;
            });
        }
        else {
            ngrok.disconnect(prevServiceUrl, function () {
                startNgrok();
            });
        }
    };
    /**
     * Applies configuration changes.
     */
    BotFrameworkService.prototype.configure = function (settings) {
        // Did ngrok path change?
        if (this.ngrokPath !== settings.framework.ngrokPath) {
            this.relaunchNgrok();
        }
    };
    return BotFrameworkService;
}(restServer_1.RestServer));
exports.BotFrameworkService = BotFrameworkService;
//# sourceMappingURL=botFrameworkService.js.map