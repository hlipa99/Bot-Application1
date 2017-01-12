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
var settings_1 = require('./settings');
var jwt = require('jsonwebtoken');
var oid = require('./OpenIdMetadata');
var BotFrameworkAuthentication = (function () {
    function BotFrameworkAuthentication() {
        var _this = this;
        this.verifyBotFramework = function (req, res, next) {
            var token;
            if (req.headers && req.headers.hasOwnProperty('authorization')) {
                var auth = req.headers['authorization'].trim().split(' ');
                ;
                if (auth.length == 2 && auth[0].toLowerCase() == 'bearer') {
                    token = auth[1];
                }
            }
            var activeBot = settings_1.getSettings().getActiveBot();
            // Verify token
            if (token) {
                var decoded = jwt.decode(token, { complete: true });
                _this.openIdMetadata.getKey(decoded.header.kid, function (key) {
                    if (key) {
                        try {
                            var verifyOptions = {
                                jwtId: activeBot.botId,
                                issuer: settings_1.authenticationSettings.tokenIssuer,
                                audience: settings_1.authenticationSettings.tokenAudience,
                                clockTolerance: 300
                            };
                            jwt.verify(token, key, verifyOptions);
                        }
                        catch (err) {
                            try {
                                // fall back to v3.0 token characteristics
                                var verifyOptions = {
                                    jwtId: activeBot.botId,
                                    issuer: settings_1.v30AuthenticationSettings.tokenIssuer,
                                    audience: settings_1.v30AuthenticationSettings.tokenAudience,
                                    clockTolerance: 300
                                };
                                jwt.verify(token, key, verifyOptions);
                            }
                            catch (err2) {
                                res.status(401);
                                res.end();
                                return;
                            }
                        }
                        next();
                    }
                    else {
                        res.status(500);
                        res.end();
                        return;
                    }
                });
            }
            else if (!activeBot.msaAppId && !activeBot.msaPassword) {
                // Emulator running without auth enabled
                next();
            }
            else {
                // Token not provided so
                res.status(401);
                res.end();
            }
        };
        this.openIdMetadata = new oid.OpenIdMetadata(settings_1.v30AuthenticationSettings.openIdMetadata);
    }
    return BotFrameworkAuthentication;
}());
exports.BotFrameworkAuthentication = BotFrameworkAuthentication;
//# sourceMappingURL=botFrameworkAuthentication.js.map