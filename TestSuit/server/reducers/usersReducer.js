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
var serverSettingsTypes_1 = require('../../types/serverSettingsTypes');
exports.usersReducer = function (state, action) {
    if (state === void 0) { state = serverSettingsTypes_1.usersDefault; }
    switch (action.type) {
        case 'Users_SetCurrentUser':
            var usersById = Object.assign({}, state.usersById);
            usersById[action.state.user.id] = action.state.user;
            return Object.assign({}, { currentUserId: action.state.user.id, usersById: usersById });
        case 'Users_AddUsers': {
            var newUsersById = {};
            for (var key in action.state.users) {
                var user = action.state.users[key];
                newUsersById[user.id] = user;
            }
            return Object.assign({}, state, { usersById: newUsersById });
        }
        case 'Users_RemoveUsers': {
        }
        default:
            return state;
    }
};
//# sourceMappingURL=usersReducer.js.map