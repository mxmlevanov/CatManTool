(function(angular) {
    "use strict";
    angular.module("app-sales").directive("tree", function() {
        return {                    
            restrict: "AE",         
            scope: {},
            templateUrl: "views/treeView.html",
            controllerAs: "tr",
            bindToController: {
                source: "=", hierarchy: "=", levelname: "=", parentname: "=", showtotal: "&", exptree:"&", togtree:"&", showgoods:"&"
            },
            controller: function($scope) {
                var t = $scope.tr;
                var cat = t.source;
                t.cattoshow = [];

               
               
                var getNotNullChildLevel = function(level) {
                    var sInd = t.hierarchy.indexOf(level);
                    for (var i = sInd+1; i < t.hierarchy.length; i++) {
                        var prop = t.hierarchy[i];
                        var check = cat.some(function(item) {
                            return item[prop] !== null;
                        });
                        if (check) {
                            return t.hierarchy[i];
                        }
                    }
                    return null;
                }

                var getChildren = function(parName, parLevel, level) {
                    var resultArr = [];
                    var tempArr = [];
                    var notDistinctArr = [];
                    if (parName.length > 0) {
                        tempArr = cat.filter(function(item) {
                            return (level !== null && item[level] !== null && item[parLevel] === parName);
                        });
                    } else {
                        tempArr = cat.filter(function(item) {
                            return (item[level] !== null);
                        });
                    }
                    tempArr.forEach(function(item, i, arr) {
                        notDistinctArr.push({
                            level: level,
                            name: item[level],
                            isChecked: false,
                            isExpanded: false
                        });
                    });
                    notDistinctArr.forEach(function(item) {
                        if (resultArr.map(function(e) { return e.name }).indexOf(item.name) === -1) {
                            resultArr.push(item);
                        }
                    });
                    return resultArr;
                }
                var levelToShow = getNotNullChildLevel(t.levelname);

                t.cattoshow = getChildren(t.parentname, t.levelname, levelToShow);
               
                t.showtotal({ info: t.cattoshow, show: true, parent: t.parentname });
              //  var tt = "test";
             //   t.showgoods({ elem: tt, set: true });

                t.exptree = function (root) {
                    root.elem.isExpanded = !root.set;
                    root.elem.isChecked = false;
                    var levToShow = getNotNullChildLevel(root.elem.level);
                    var ctshow = getChildren(root.elem.name, root.elem.level, levToShow);
                    t.showtotal({ info: ctshow, show: root.set, parent: root.elem });

                }

                t.togtree = function (root) {
                    var test = root;
             //       root.elem.isChecked = !root.set;
                    t.showgoods({ elem: root.elem, set: root.set });
                    t.$emit("test0","test0");

                }
               
                
            },
        
            link: function (scopе, elem, attr, ctr) {
                scopе.$emit("test","test");
            }
        }
    });
}(angular));