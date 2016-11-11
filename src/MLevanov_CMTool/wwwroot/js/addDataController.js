(function(angular) {
    "use strict";
    angular.module("app-sales").controller("addDataController",
        function($scope, $http) {
            var ad = this;
            ad.names = [];
            ad.dataTypes = [
                {
                    dataType: "stores",
                    label: 'Магазины'
                },
                {
                    dataType: "products",
                    label: 'Продукты'
                },
                {
                    dataType: "sales",
                    label: 'Продажи'
                }
            ];
            ad.data = {};
            ad.tempData = [];
            ad.toPost = {};
            ad.addColName = function (col, ind) {
                var ttt = ind;
                if (ad.data.selectedItems.length > ind) {
                    ad.data.selectedItems[ind].xlsCol = col;
                    return col;
                } else {
                    ad.data.selectedItems.push({
                        id: ind, dbCol: 'notActual', label: 'Не используется', xlsCol: col
                    });
                    return col;
                }
            }
            ad.removeDataFromDb = function() {
                $http.post("/api/datas/delete");
            }
            ad.addDataToDb = function () {
                var i = 0;
                ad.tempData = [];
                ad.xlsdata.forEach(function(row) {
                    ad.tempData[i] = { id: i };
                    ad.data.selectedItems.forEach(function(item) {
                        if (item.dbCol !== 'notActual') {
                            for (var key in row) {
                                if (row.hasOwnProperty(key)) {
                                    if (key === item.xlsCol) {
                                        var ttt = item.dbCol;
                                        ad.tempData[i][ttt] = row[key];
                                    }
                                }
                            }
                        }
                    });
                    i++;
                });
                //var test = { Id: 1, Store: 'somestore', GoodsClass: 'someclass', SectionsNumber: 1, ShelvesNumber: 1, ShelvesWidth: 1 };
                //var tempMode = JSON.stringify(ad.data.mode);
                ad.toPost = { mode: ad.data.mode, data: ad.tempData }
                $http.post("/api/datas",ad.toPost)
               .then(function () {

                }, function () {

                });

            };
            ad.getSelectedItem = function (item,ind,col) {              
                ad.data.selectedItems[ind] = item;
                ad.data.selectedItems[ind].xlsCol = col;
            };
            ad.getMode = function(mode) {
                ad.data.mode = mode;

                switch (mode) {
                case 'stores':
                    
                    ad.names = [
                          {
                              id: '1',
                              dbCol: 'storeCode',
                              label: 'Код магазина'
                          },
                        {
                            id: '2',
                            dbCol: 'shelveStore',
                            label: 'Название магазина'
                        },
                        {
                            id: '3',
                            dbCol: 'shelveGoodsClass',
                            label: 'Категория'
                        },
                        {
                            id: '4',
                            dbCol: 'sectionsNumber',
                            label: 'Количество секций'
                        },
                        {
                            id: '5',
                            dbCol: 'shelvesNumber',
                            label: 'Количество полок'
                        },
                        {
                            id: '6',
                            dbCol: 'shelvesWidth',
                            label: 'Ширина полок см.'
                        }
                    ];
                    break;
                case 'products':
                    ad.names = [
                        {
                            id: '1',
                            dbCol: 'code',
                            label: 'Код продукта'
                        },
                        {
                            id: '2',
                            dbCol: 'name',
                            label: 'Название продукта'
                        },
                        {
                            id: '3',
                            dbCol: 'goodsClass',
                            label: 'Категория'
                           
                        },
                        {
                            id: '4',
                            dbCol: 'width',
                            label: 'Ширина пачки см.'
                        },
                        {
                            id: '5',
                            dbCol: 'minPack',
                            label: 'Минимальная упаковка шт.'
                        }
                    ];
                    break;
                case 'sales':
                    ad.names = [
                        {
                            id: '0',
                            dbCol: 'storeCode',
                            label: 'Код магазина продажи'
                        },
                        {
                            id: '1',
                            dbCol: 'productCode',
                            label: 'Код товара'
                        },
                        {
                            id: '2',
                            dbCol: 'name',
                            label: 'Наименование товара'
                        },
                        {
                            id: '3',
                            dbCol: 'goodClass',
                            label: 'Категория'
                        },
                        {
                            id: '4',
                            dbCol: 'week',
                            label: 'Неделя продаж'
                        },
                        {
                            id: '5',
                            dbCol: 'salespcs',
                            label: 'Продажи шт.'
                        },
                        {
                            id: '6',
                            dbCol: 'salesPrice',
                            label: 'Цена продажи'
                        },
                        {
                            id: '7',
                            dbCol: 'purchasingPrice',
                            label: 'Цена закупки'
                        }
                    ];
                    break;

                default:
                    ad.names = [];

                };
                ad.names.push({ id: '0', dbCol: 'notActual', label: 'Не используется' });
                ad.data.selectedItems = ad.names;
            };
        });

}(angular));
   