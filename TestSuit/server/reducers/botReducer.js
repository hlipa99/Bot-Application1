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
var utils_1 = require('../../utils');
var settings_1 = require('../settings');
exports.botsReducer = function (state, action) {
    if (state === void 0) { state = []; }
    switch (action.type) {
        case 'Bots_AddOrUpdateBot': {
            var botId_1 = action.state.bot.botId || utils_1.uniqueId();
            var settings = settings_1.getSettings();
            if (settings.bots.find(function (value) { return value.botId === botId_1; })) {
                botId_1 = utils_1.uniqueId();
            }
            var index = state.findIndex(function (value) { return value.botId === action.state.bot.botId; });
            if (index >= 0) {
                return state.slice(0, index).concat([
                    Object.assign({}, action.state.bot, { botId: state[index].botId })
                ], state.slice(index + 1));
            }
            else {
                return state.concat([
                    Object.assign({}, action.state.bot, { botId: botId_1 })
                ]);
            }
        }
        case 'Bots_RemoveBot': {
            return state.filter(function (value) { return value.botId !== action.state.botId; });
        }
        default:
            return state;
    }
};
exports.activeBotReducer = function (state, action) {
    if (state === void 0) { state = ''; }
    switch (action.type) {
        case 'ActiveBot_Set':
            return action.state.botId || state;
        default:
            return state;
    }
};
//# sourceMappingURL=botReducer.js.map