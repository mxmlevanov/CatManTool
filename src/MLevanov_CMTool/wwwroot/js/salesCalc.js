(function(angular) {
    "use strict";

    angular.module("app-sales").
        factory("salesCalc", salesCalc());

    function salesCalc() {
        var setSalesProfile = function(week, goodsList) {
            var weeklySales = goodsList.map(function(good) {
                return good.goodSales;
            }).filter(function(sale) {
                return sale.week === week;
            }).map(function(item) {
               return  item.salespcs;
            }).reduce(function(a, b) {
                return a + b;
            });
            goodsList.forEach(function(good) {
                var ts = good.goodSales.find(function(sale) {
                    return sale.week === week;
                });
                ts.range = Math.round(ts.salespcs*100 / weeklySales)/100;
            });
        }
        
    
        return {
            editProfile: function(clickedGoodOrCategory, goodsList, mark) {
                if (mark === "rating") {
                    if (clickedGoodOrCategory.code.length > 0) { // this is category
                        var classesOnLevel = goodsList.filter(function(item) {
                            return item.parent.name === clickedGoodOrCategory.parent.name && item.parent.level === clickedGoodOrCategory.parent.level;
                        });  
                        if (classesOnLevel.length > 1) {
                            classesOnLevel.sort(function(a, b) { return b.rating - a.rating });
                            classesOnLevel.forEach(function(item) {
                                if (item.rating <= clickedGoodOrCategory.rating) {
                                }
                            });
                        }
                    }
                }
            },
            setProfile: function (category, goodsList, activeWeek) {
                var startWeek = ((activeWeek - 6) > 0) ? (activeWeek - 6) : (activeWeek + 46);
                if (category.code.length>0) {
                    var itemsOnLevel = goodsList.filter(function (item) {
                        return  item.parent.level === category.parent.level;
                    });
                    setSalesProfile(activeWeek, itemsOnLevel);
                }
            }
        }
    }

}(angular));
