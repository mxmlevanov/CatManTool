(function (angular) {
    "use strict";

    angular.module("app-sales", ["ngRoute"])
    .config(function ($routeProvider, $logProvider) {
            $logProvider.debugEnabled(true);
        $routeProvider.when("/sales", {
            controller: "salesController",
            controllerAs: "vm",
            templateUrl:"/views/sale.html"

        });
        $routeProvider.when("/addData", {
            controller: "addDataController",
            //controllerAs: "dd",
            templateUrl: "/views/addDataView.html"
        });

        
        $routeProvider.otherwise({ redirectTo: "/" });
    });

}(angular));