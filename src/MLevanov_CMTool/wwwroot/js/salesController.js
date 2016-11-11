(function(angular) {
    "use strict";

    angular.module("app-sales").
        controller("salesController", salesController).
        factory("getHttp", getHttp).
        directive("loading",loading);

    function loading() {
        return {
            restrict: "E",
            scope: {
                show:"="
            },
            templateUrl: "views/loading.html"
          
        }
    }
    


    function getHttp($http) {     
        return {
            sales: function (category) {                
                return $http.get("api/sales", { params: { categoryName: category} });             
            },
            categories: function () {
                return $http.get("api/sales/categories");
            },
            catTotalSales:function(lev, catList) {
                return $http.get("api/sales/total", { params: { level: lev, categoryList: catList } });
            }
        } 
     }

    function salesController($scope, $http, getHttp) {
        var vm = this;
        vm.selectedGoods = [];
        vm.catToShow = [];
        vm.newGoodsClass = "";
        vm.goodsClasses = ["testclass1","testclass2"];
        vm.loading = true;
        vm.goods = [];
        vm.catLoaded=false;
        vm.showSideMenu = true;
        vm.parentname = "";
        vm.hierarchy = ["section", "category", "group", "segment", "className"];
       
        getHttp.categories().then(function(responce) {
           vm.loading = false;
           vm.catLoaded = true;
           vm.categories = responce.data;
           vm.categories.forEach(function(item) {
               item.isSelected = false;
               item.isExpanded = false;
           });
        });

        vm.$on("test", function(event, data) {
            var ttt = data;
        });
        vm.$on("test0", function (event, data) {
            var tt = data;
        });

        vm.expandTree = function (elem,set) {
            var ttt = elem;
            var ttttt = set;
        }

        vm.setGoodsClass = function (nClass) {
            if (nClass !== "") {
                vm.loading = true;
                var selectedGoodCodes = vm.selectedGoods.map(function(item) {
                    return item.code;
                });
                selectedGoodCodes.unshift(nClass);
                $http.post("/api/sales/setclasses", selectedGoodCodes).then(function() {
                    vm.goods.forEach(function(item) {
                        if (item.isSelected) {
                            if (item.goodsClass !== null) {
                                item.goodsClass.className = nClass;
                            }
                            item.isSelected = false;
                        }
                    });
                    nClass = "";
                    vm.loading = false;
                    vm.selectedGoods = [];
                    vm.goodsClasses.push(vm.newGoodsClass);
                    vm.newGoodsClass = "";
                }, function() {
                });

            }

        }
        vm.startSelect = function (event, ind) {
            event.preventDefault();
            vm.goods[ind].isSelected = !vm.goods[ind].isSelected;        
            vm.msDown = true;
        }
        vm.endSelect = function(event, ind) {
            vm.msDown = false;
            // prepare list of existing names of goodClasses
            var classNames = vm.goods.map(function (item) {
                return (item.goodsClass !== null) ? item.goodsClass.className : null;                
            });
            classNames.forEach(function (item) {
                if (vm.goodsClasses.indexOf(item) === -1 && item !== null) {
                    vm.goodsClasses.push(item);
                }
            });
            // prepare list of selected good 
            vm.selectedGoods = vm.goods.filter(function(item) {
                return  (item.isSelected && item.code !== "aggregated");
            });
          
 
        }
        vm.rowSelection = function (event, ind) {
            event.preventDefault();
            if (vm.msDown) {
                vm.goods[ind].isSelected = true;
            }
        }

        vm.ifAnyGoodsSelected = function () {
          
           return  vm.goods.some(function(item) {
               return  item.isSelected;
           });
          
        }

        //vm.selectClass=function() {
        //     vm.mdVis = true;
        //}
        var removeCategoryData = function(catNameList) {
            vm.goods = vm.goods.filter(function (item) {
                if (item.goodsClass === null && catNameList.indexOf(item.name)>0)                    
                        return false;
                return true;
            });
        }
        var removeChildGoods = function (parent) {
            vm.goods = vm.goods.filter(function (item) {
                if (item.goodsClass !== null)
                    if (item.goodsClass[parent.level] === parent.name)
                        return false;
                return true;
            });
        }
        vm.showCategoryData = function (info, show, parent) {
            if (typeof info !== "undefined" && info !== null && info.length > 0) {
                var lvl = info[0].level;
                var categoryList = info.map(function(item) {
                    return item.name;
                });
                

                getHttp.catTotalSales(lvl, categoryList).then(function(res) {
                    if (vm.goods.length === 0) {
                        angular.copy(res.data, vm.goods);
                        vm.goods.forEach(function(item) {
                            item.isSelected = false;
                        });
                    } else {
                        // clear rows which left into root level from previos iterations
                        removeChildGoods(parent);
                        removeCategoryData(categoryList);
                        // search position to insert new rows
                        function ch(element) {
                            return element.name.includes(parent.name);
                        }
                        var targetInd = vm.goods.findIndex(ch);
                       // insert new rows below the corresponding level in main table
                        vm.goods.splice.apply(vm.goods, [targetInd + 1, 0].concat(res.data));
                        vm.goods.forEach(function(item) {
                            item.isSelected = false;
                        });
                    }
                });
            }
        }
      

        vm.showCategoryGoodsData = function (elem, set) {
            // clear rows which left into root level from previos iterations
            if (elem !== "test") {


                removeChildGoods(elem);

                // search position to insert new rows
                function check(element) {
                    return element.name.includes(elem.name);
                }

                var targetInd = vm.goods.findIndex(check);
                // get fresh data and insert new rows above the corresponding root level in main table
                if (!elem.isChecked) {
                    getHttp.sales(elem.name).then(function(res) {
                        res.data.forEach(function(item) {
                            item.isSelected = false;
                        });
                        vm.goods.splice.apply(vm.goods, [targetInd, 0].concat(res.data));
                        elem.isChecked = !set;
                    });
                }
                elem.isChecked = !set;
            }
        }

     
                
            
        $scope.saveChanges = function(updsale,goodid) {  
            var result = [goodid, updsale.id, updsale.salespcs];    
            $http.post("/api/sales", result).then(function() {
            }, function() {
            });
        }
    };
    

}(angular));
