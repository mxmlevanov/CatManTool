(function(angular) {
    "use strict";
    angular.module("app-sales").
        service("nodeData",nodeData).
        directive("tree", tree).
        directive("node", node);

    function nodeData() {
        this.getChildren = function (parName, parLevel,cat, hierarchy) {
            var getNotNullChildLevel = function(level) {
                var sInd = hierarchy.indexOf(level);
                for (var i = sInd + 1; i < hierarchy.length; i++) {
                    var prop = hierarchy[i];
                    var check = cat.some(function(item) {
                        return item[prop] !== null;
                    });
                    if (check) {
                        return hierarchy[i];
                    }
                }
                return null;
            }
            var level = getNotNullChildLevel(parLevel);
            var resultArr = [];
            var notDistinctArr = [];       
            var tempArr = cat.filter(function(item) {
                    return (level !== null && item[level] !== null && (item[parLevel] === parName || item[parLevel]==="initial"));
                });          
            tempArr.forEach(function(item) {
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
            return  resultArr;
        }

    }

    function tree() {
        return {                    
            restrict: "E",         
            scope: {},
            templateUrl: "views/treeView.html",
            controllerAs: "tr",
            bindToController: {
                source: "=", hierarchy: "=", showtotal: "&", showgoods:"&"
            },
            controller: function () {            
            }
        }
    }

    function node(nodeData) {
        return {
            require:"^tree",
            restrict: "E",
            scope: {},
            templateUrl: "views/nodeView.html",
            controllerAs: "nd",
            bindToController: {
               source:"=",hierarchy:"=", name:"=",level:"="
            },
            controller: function ($scope) {
                $scope.nd.childtoshow = nodeData.getChildren($scope.nd.name, $scope.nd.level, $scope.nd.source, $scope.nd.hierarchy);
            },
            compile: function() {
                return {
                    pre: function(scopе, elem, attr, ctr) {
                        var n = scopе.nd;
                        if (n.level === "total") {
                            var total = {
                                name: "totalGoods",
                                level: "total",
                                isChecked: false,
                                isExpanded: true
                            }
                            ctr.showtotal({ info: n.childtoshow, show: true, parent: total });
                        }
                        n.exptree = function(root) {
                            var childcats = nodeData.getChildren(root.elem.name, root.elem.level, n.source, n.hierarchy);
                            ctr.showtotal({ info: childcats, show: root.set, parent: root.elem });
                            root.elem.isExpanded = !root.set;
                            root.elem.isChecked = false;
                            childcats.forEach(function(cat) {
                                cat.isChecked = false;
                            });
                        }
                        n.togtree = function(root) {
                            ctr.showgoods({ elem: root.elem, set: root.set });
                            root.elem.isChecked = !root.set;
                        }
                    }
                }
            }
        }
    }
}(angular));