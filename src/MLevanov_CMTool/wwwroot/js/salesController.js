(function(angular) {
    "use strict";

    angular.module("app-sales").
        controller("salesController", salesController).
        factory("getHttp", getHttp).
        directive("loading",loading).
        directive('contenteditable', contenteditable);

    function loading() {
        return {
            restrict: "E",
            scope: {
                show:"="
            },
            templateUrl: "views/loading.html"         
        }
    }

    function contenteditable($sce) {
        return {
            restrict: 'A', 
            require: '?ngModel',
         
            link: function(scope, element, attrs, ngModel) {
                if (!ngModel) return; 
                ngModel.$render = function() {
                    element.html($sce.getTrustedHtml(ngModel.$viewValue || ''));
                };
               // Listen for change events to enable binding
                element.on("blur keydown", function (event) {
                    var ind = element[0].cellIndex;
                    if (event.type === "blur" || event.keyCode === 13 || event.keyCode === 40 || event.keyCode === 38 || event.keyCode === 39 || event.keyCode === 37) {
                        if (event.keyCode === 13 || event.keyCode === 40) {
                            event.preventDefault();
                            element[0].parentElement.nextElementSibling.children[ind].focus();
                        } else if (event.keyCode === 38) {
                            event.preventDefault();
                            element[0].parentElement.previousElementSibling.children[ind].focus();
                        } else if (event.keyCode === 39) {
                           event.preventDefault();
                           element[0].nextElementSibling.focus();
                        } else if (event.keyCode === 37) {
                            event.preventDefault();
                            element[0].previousElementSibling.focus();
                        }
                        scope.$evalAsync(read);           
                        scope.$parent.vm.calcForecast(scope.good, attrs.mark);
                    }
                });
                read();
                function read() {
                    var html = element.html();
                    if (html === "<br>") {
                        html = "";
                    }
                    if (html.slice(-8) === "<br><br>") {
                        html = html.slice(0, -8);
                    }
                    ngModel.$setViewValue(html);
                }
            }
        };
    }

    function getHttp($http,$q) {     
        return {
            goodSales: function (lev,category) {                
                return $http.get("api/sales", { params: {categoryLevel:lev, categoryName: category} });             
            },
            catTotalSales:function(lev, catList) {
                return $http.get("api/sales/totalbycategories", { params: { level: lev, categoryList: catList } });
            },
            initial:function($q) {
                var totalSale = $http.get("api/sales/total");
                var categories = $http.get("api/sales/categories");
                return $q.all([totalSale, categories]);
            }
        } 
     }

    function salesController($scope, $http, getHttp, salesCalc, $q) {
        var vm = this;
        vm.selectedGoods = [];
        vm.ag = true;
        vm.catToShow = [];
        vm.newGoodsClass = "";
        vm.goodsClasses = ["testclass1","testclass2"];
        vm.loading = true;
        vm.goods = [];
        vm.catLoaded=false;
        vm.showSideMenu = true;
        vm.parentname = "";
        vm.hierarchy = ["total","section", "category", "group", "segment", "className"];

        var processWeeks = function(goodList) {
            vm.weeks = [];
            var minWeek = 1000000;
            var maxWeek = 0;
            goodList.forEach(function(good) {
                good.goodSales.forEach(function(sale) {
                    if (sale.week < minWeek) {
                        minWeek = sale.week;
                    }
                    if (sale.week > maxWeek) {
                        maxWeek = sale.week;
                    }
                });
            });
            for (var i = minWeek; i <= maxWeek; i++) {
                vm.weeks.push(String(i));
            }
        }
        var arrangeGoodsArray = function() {
            processWeeks(vm.goods);
            vm.goods.forEach(function (good) {
                vm.weeks.forEach(function (wk) {
                    if (good.goodSales.map(function (e) { return e.week }).indexOf(wk) === -1) {
                        good.goodSales.push({
                            name: null, productCode: null, purchasingPrice: 0,
                            salesPrice: 0, salespcs: 0, storeCode: null, week: wk, range: 0, forecast: 0
                        });
                    }
                });
                good.goodSales.sort(function (a, b) { return a.week - b.week });
            });
        }       

        getHttp.initial($q).then(function (response) {
            vm.loading = false;
            vm.catLoaded = true;
            vm.goods.push(response[0].data);
       //     angular.copy(response[0].data, vm.goods[0]);
            vm.goods.forEach(function (item) {
                item.parent = "initial";
                item.goodSales.sort(function (a, b) { return a.week - b.week });
                item.isSelected = false;
            });
            arrangeGoodsArray();
            vm.categories = response[1].data;
            vm.categories.forEach(function (item) {
                item.isSelected = false;
                item.isExpanded = false;
            });
          
        });
        

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
        vm.endSelect = function() {
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
        var removeCategoryData = function(catNameList) {
            vm.goods = vm.goods.filter(function (item) {
                if (item.goodsClass === null && catNameList.indexOf(item.name) !== -1)                    
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

        //vm.calcForecast = function (currentGood, mark) {
        //    if (mark === "rating") {
        //        if (currentGood.code.length > 0) { // this is aggregated level
        //            var classesOnLevel = vm.goods.filter(function(item) {
        //                return item.parent.name === currentGood.parent.name && item.parent.level === currentGood.parent.level;
        //            });
        //            var test = classesOnLevel;
        //            if (classesOnLevel.length>1) {
        //                classesOnLevel.sort(function(a, b) {return  b.rating - a.rating });
        //                classesOnLevel.forEach(function(item) {
        //                    if (item.rating <= currentGood.rating) {
                                
        //                    }
        //                });
        //            }
        //        }

        //    }
            
          
        //        var test = event;
        //        var test1 = currentGood;
            
        //}
        
           

        vm.showCategoryData = function (info, show, parent) {
            if (typeof info !== "undefined" && info !== null && info.length > 0) {
                var lvl = info[0].level;
                var categoryList = info.map(function(item) {
                    return item.name;
                });               
                getHttp.catTotalSales(lvl, categoryList).then(function(res) {
                    if (vm.goods.length === 0) {
                        angular.copy(res.data, vm.goods);
                        vm.goods.forEach(function (item) {
                            item.parent = parent;
                            item.goodSales.sort(function(a, b) { return a.week - b.week });
                            item.isSelected = false;                          
                        });
                        arrangeGoodsArray();
                    } else {
                        // clear rows which left into root level from previos iterations
                        removeChildGoods(parent);
                        removeCategoryData(categoryList);
                        // search position to insert new rows
                        if (parent.isExpanded === true) {
                            function ch(element) {
                                return element.name.includes(parent.name);
                            }
                            var targetInd = vm.goods.findIndex(ch);
                            // insert new rows below the corresponding level in main table

                            res.data.forEach(function(good) {
                                good.parent = parent;
                                good.goodSales.sort(function (a, b) { return a.week - b.week });
                            });
                            processWeeks(res.data);
                            vm.weeks.forEach(function(week) {
                                salesCalc.setProfile(parent, res.data, week);
                            });
                        
                            vm.goods.splice.apply(vm.goods, [targetInd + 1, 0].concat(res.data));
                            vm.goods.forEach(function (item) {
                                item.isSelected = false;
                            });
                            arrangeGoodsArray();
                        }
                    }
                });
            }
        }     

        vm.showCategoryGoodsData = function (elem, set) {
                removeChildGoods(elem);
                // get fresh data and insert new rows above the corresponding root level in main table
                function check(element) {
                    return element.name.includes(elem.name);
                }
                var targetInd = vm.goods.findIndex(check);
                if (!elem.isChecked) {
                    getHttp.goodSales(elem.level, elem.name).then(function(res) {
                        res.data.forEach(function(item) {
                            item.isSelected = false;
                            item.parent = elem;
                        });
                        vm.goods.splice.apply(vm.goods, [targetInd+1, 0].concat(res.data));
                        arrangeGoodsArray();                        
                    });                 
                }
                elem.isChecked = !set;            
        }
          
        $scope.saveChanges = function(updsale,goodid) {  
            var result = [goodid, updsale.id, updsale.salespcs];    
            $http.post("/api/sales", result).then(function() {
            }, function() {
            });
        }
    };
    

}(angular));
