System.register(['vue'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var vue_1;
    var vm;
    return {
        setters:[
            function (vue_1_1) {
                vue_1 = vue_1_1;
            }],
        execute: function() {
            vm = new vue_1.default({
                el: 'main'
            });
        }
    }
});
//# sourceMappingURL=site.js.map