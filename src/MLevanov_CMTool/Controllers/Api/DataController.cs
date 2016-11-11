using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using MLevanov_CMTool.Models;
using MLevanov_CMTool.ViewModels;
using Newtonsoft.Json.Linq;
using Enumerable = System.Linq.Enumerable;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MLevanov_CMTool.Controllers.Api
{
    public class DataController : Controller
    {
       
        private ICmToolRepository _repository;
        public DataController(ICmToolRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("api/datas/delete")]
        public void Delete()
        {
            _repository.ClearData();
        }

        [HttpPost("api/datas")]
        public void Post([FromBody] JObject postedData)
        {
            string mode = postedData["mode"].ToObject<string>();

           
            switch (mode)
            {
                case "stores":
                {


                    List<ShelvesViewModel> shelves = postedData["data"].ToObject<List<ShelvesViewModel>>();
                    var result = Mapper.Map<List<Shelve>>(shelves);
                    _repository.AddShelves( result );
                    }
                    break;
                case "products":
                {
                        List<ProductViewModel> products = postedData["data"].ToObject<List<ProductViewModel>>();
                        var result = Mapper.Map<List<Good>>(products);
                        _repository.AddProducts(result);
                    }
                    break;
                case "sales":
                {
                        List<SaleViewModel> sales = postedData["data"].ToObject<List<SaleViewModel>>();
                         var result = Mapper.Map <List<Sale>>(sales);
                        _repository.AddSales(result);
                    //    List<Good> resGoods = new List<Good>();
                    //var goodList = sales.GroupBy(x => x.Id);
                    //int i = 0;
                    //foreach (IGrouping<int,SaleViewModel> g in goodList)
                    //{
                    //    var firstOrDefault = g.FirstOrDefault(x=>x.Id==g.Key);
                    //    if (firstOrDefault != null)
                    //        resGoods.Add( new Good()
                    //        {
                    //            Code = firstOrDefault.ProductCode,
                    //            Name = firstOrDefault.Name,
                    //            Sales = new List<Sale>()

                    //        });
                    //    foreach (var s in g)
                    //    {
                    //            Store nst = new Store() { StoreCode = s.StoreCode };
                    //            Sale ns = new Sale()
                    //            {
                    //                PurchasingPrice = s.PurchasingPrice,
                    //                SalesPrice = s.SalesPrice,
                    //                Salespcs = s.Salespcs,
                    //                Store = nst,
                    //                Week = s.Week

                    //            };

                    //            resGoods[i].Sales.Add(ns);
                    //    }
                    //}
                    //var t = goodList;
                    //var tt = goodList;

                }
                    break;
                default:
              
                    break;
            }
          
        }
    }
}
