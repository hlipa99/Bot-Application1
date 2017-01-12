"use strict";
var assert = require('assert');
function Test1A() {
    assert.ok(true, "This shouldn't fail");
}
exports.Test1A = Test1A;
function Test2BS() {
    assert.ok(1 === 1, "This shouldn't fail");
    assert.ok(false, "This should fail");
}
exports.Test2BS = Test2BS;
//# sourceMappingURL=UnitTest1.js.map