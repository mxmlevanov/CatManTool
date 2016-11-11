using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLevanov_CMTool.Models
{
    public class CmToolSeedData
    {
        private CmToolContext _context;

        public CmToolSeedData(CmToolContext context)
        {
            _context = context;
        }

        //public void EnsureSeedingData()

        //{
           
        //    if (_context.Goods.Any()) return;
        //    var salesL = new List<Sale>()
        //    {
        //        new Sale() {Week = new DateTime(2016, 01, 10), Salespcs = 10,Forecast = 10,Range = 1,SalesPrice = 55},
        //        new Sale() { Week = new DateTime(2016, 01, 17), Salespcs = 20,Forecast = 20,Range = 1,SalesPrice = 55},
        //        new Sale() {Week = new DateTime(2016, 01, 21), Salespcs = 30,Forecast = 30,Range = 1,SalesPrice = 55},
        //        new Sale() {Week = new DateTime(2016, 01, 28), Forecast = 25,Range = 2,SalesPrice = 55}
        //    };          
        //    var liptonTea = new Good() {Name = "Lipton 25 tb", Code = 1,Sales = salesL};
        //    _context.Goods.Add(liptonTea);
        //    _context.Sales.AddRange(liptonTea.Sales);

        //    var salesD = new List<Sale>()
        //    {
        //        new Sale() {Week = new DateTime(2016, 01, 10), Salespcs = 5,Forecast = 5,Range = 2,SalesPrice = 70},
        //        new Sale() { Week = new DateTime(2016, 01, 17), Salespcs = 10,Forecast = 10,Range = 2,SalesPrice = 70},
        //        new Sale() {Week = new DateTime(2016, 01, 21), Salespcs = 20,Forecast = 20,Range = 2,SalesPrice = 70},
        //        new Sale() {Week = new DateTime(2016, 01, 28), Forecast = 20,Range =31,SalesPrice = 70}
        //    };

        //    var dilmahTea = new Good() {Name = "Dilmah 100g", Code = 2,Sales = salesD};
        //    _context.Goods.Add(dilmahTea);
        //    _context.Sales.AddRange(dilmahTea.Sales);

        //    var salesS = new List<Sale>()
        //    {
        //        new Sale() {Week = new DateTime(2016, 01, 10), Salespcs = 7,Forecast = 7,Range = 3,SalesPrice =100},
        //        new Sale() { Week = new DateTime(2016, 01, 17), Salespcs = 14,Forecast = 15,Range = 3,SalesPrice = 100},
        //        new Sale() {Week = new DateTime(2016, 01, 21), Salespcs = 21,Forecast = 20,Range = 3,SalesPrice = 100},
        //        new Sale() {Week = new DateTime(2016, 01, 28), Forecast = 40,Range = 1,SalesPrice = 50}
        //    };
        //    var sheryTea = new Good() {Name = "Shery 25 tb", Code = 3,Sales = salesS};          
        //    _context.Goods.Add(sheryTea);
        //    _context.Sales.AddRange(sheryTea.Sales);

        //    _context.SaveChanges();
        //}

    }
}