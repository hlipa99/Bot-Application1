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
var emulator_1 = require('./emulator');
var sendMessage = function (method, message) {
    var args = [];
    for (var _i = 2; _i < arguments.length; _i++) {
        args[_i - 2] = arguments[_i];
    }
    emulator_1.Emulator.send.apply(emulator_1.Emulator, [method, message].concat(args));
};
exports.log = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-log', message].concat(args));
};
exports.info = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-info', message].concat(args));
};
exports.trace = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-trace', message].concat(args));
};
exports.debug = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-debug', message].concat(args));
};
exports.warn = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-warn', message].concat(args));
};
exports.error = function (message) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    sendMessage.apply(void 0, ['log-error', message].concat(args));
};
exports.makeLinkMessage = function (text, link, title) {
    return {
        messageType: 'link',
        text: text,
        link: link,
        title: title
    };
};
exports.ngrokConfigurationLink = function (text) {
    return exports.makeLinkMessage(text, 'emulator://appsettings?tab=NgrokConfig');
};
exports.botCredsConfigurationLink = function (text) {
    return exports.makeLinkMessage(text, 'emulator://botcreds');
};
exports.api = function (operation, req, res, request, response, text) {
    if (res.statusCode >= 400) {
        exports.error('<-', exports.makeInspectorLink("" + req.method, request), exports.makeInspectorLink("" + res.statusCode, response, "(" + res.statusMessage + ")"), operation, text);
    }
    else {
        exports.info('<-', exports.makeInspectorLink("" + req.method, request), exports.makeInspectorLink("" + res.statusCode, response, "(" + res.statusMessage + ")"), operation, text);
    }
};
exports.makeInspectorLink = function (text, obj, title) {
    if (typeof (obj) === 'object' || Array.isArray(obj)) {
        var json = JSON.stringify(obj);
        return exports.makeLinkMessage(text, "emulator://inspect?obj=" + encodeURIComponent(json), title);
    }
    else {
        return text;
    }
};
exports.makeCommandLink = function (text, args, title) {
    var json = JSON.stringify(args);
    return exports.makeLinkMessage(text, "emulator://command?args=" + encodeURIComponent(json), title);
};
//# sourceMappingURL=log.js.map