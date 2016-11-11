(function(angular) {
    "use strict";

    angular.module("app-sales").directive("showXls", function() {
        return {
            controller: 'addDataController',
            controllerAs: 'ad',
            bindToController: true,
            link: function($scope, elem) {
                elem.bind("change", function(e) {
                    $scope.ad.xlsdata = [];
                    $scope.ad.colNames = [];
                    $scope.ad.file = (e.srcElement || e.target).files[0];
                    var reader = new FileReader();
                    reader.onload = function() {
                        $scope.$apply(function() {
                            var data = reader.result;
                            var workbook = XLSX.read(data, { type: "binary" });
                            $scope.ad.sheetNames = workbook.SheetNames;
                            workbook.SheetNames.forEach(function(sheetName) {
                                var roa = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                                if (roa.length > 0) {

                                    $scope.ad.xlsdata = roa;

                                }

                                function getColNames(t) {
                                    for (var key in t) {
                                        $scope.ad.colNames.push(key);
                                    }
                                }

                                getColNames(roa[0]);
                            });

                        });

                    };
                    reader.readAsBinaryString($scope.ad.file);

                });
            }
        }
    });


}(angular));