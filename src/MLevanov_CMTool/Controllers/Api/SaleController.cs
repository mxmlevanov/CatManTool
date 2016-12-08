using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using MLevanov_CMTool.Models;
using MLevanov_CMTool.ViewModels;
using Microsoft.AspNet.Hosting;

namespace MLevanov_CMTool.Controllers.Api
{
    public class SaleController
    {
        private ICmToolRepository _repository;
      
        public SaleController(ICmToolRepository repository)
        {
            _repository = repository;
        }
 
        [HttpGet("api/sales")]
        public JsonResult Get(string categoryLevel, string categoryName)
        {
            IEnumerable<Good> goodList = _repository.GetGoodsWithWeeklySales(categoryLevel, categoryName);        
            var results = Mapper.Map<IEnumerable<GoodViewModel>>(goodList);
            var t = new JsonResult(results);
            return t;
        }

        [HttpGet("api/sales/total")]
        public JsonResult GetTotal()
        {
            Good totalGood = _repository.GetSalesTotal();
            var results = Mapper.Map<GoodViewModel>(totalGood);
            var t = new JsonResult(results);
            return t;
        }
        [HttpGet("api/sales/totalbycategories")]
        public JsonResult GetTotalByCategories (string level, string[] categoryList)
        {
            IEnumerable<Good> goodList =  _repository.GetSalesTotalByCategories(level, categoryList);
            var results = Mapper.Map<IEnumerable<GoodViewModel>>(goodList);
            var t = new JsonResult(results);
            return t;
        }
        [HttpGet("api/sales/categories")]
        public JsonResult GetCategories()
        {
           IEnumerable<GoodsClass> goodsClasses = _repository.GetGoodsClasses();
            var t = goodsClasses;
            return new JsonResult(goodsClasses);           
        }
        [HttpPost("api/sales")]
        public  void Post([FromBody]int[] svm)
        {
            var goodId = svm[0];
            var saleId = svm[1];
            var salePsc = svm[2];
            _repository.UpdateSaleById(goodId,saleId,salePsc);
        }
        [HttpPost("api/sales/setclasses")]
        public void PostClasses ([FromBody]string[] classesData)
        {
            string className = classesData[0];
            List<string> goodCodes = classesData.Skip(1).ToList();
            _repository.SetGoodsClass(className,goodCodes);
        }
    }
}
