(function (angular) {
    "use strict";
    angular.module("app-sales").directive("modal", function () {
        return {
            restrict: "AE",
            scope: {},
            templateUrl: "views/modalSelectView.html",        
            controllerAs: "md",
            bindToController: {
               vsb:"="
            },
            controller: function ($scope) {
                var md = this;
                var ttt = md.vsb;
                md.test = "test100";


            },

            link: function (scopе, elem, attr, ctr) {

            }
        }
    });
}(angular));