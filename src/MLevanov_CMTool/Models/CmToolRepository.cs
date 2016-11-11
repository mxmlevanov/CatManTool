using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Query.ResultOperators.Internal;
using MLevanov_CMTool.ViewModels;

namespace MLevanov_CMTool.Models
{
    public interface ICmToolRepository
    {
        IEnumerable<Good> GetGoodsWithWeeklySales(string catName);
        void AddShelves (List<Shelve> data);
        void AddProducts (List<Good> data);
        void AddSales(List<Sale> data);
        void UpdateSaleById(int goodId,int saleId, int salePsc);
        void SetGoodsClass(string className, List<string> goodCodes);
        void ClearData();
        IEnumerable<GoodsClass> GetGoodsClasses();
        IEnumerable<Good> GetSalesTotal(string level, string[] categoryName);
    }

    public static class MyExt
    {
       
        public static GoodsClass GetGoodsClass(this CmToolContext cont, string catName)
        {
            GoodsClass updGoodsClass = cont.GoodsClasses
                .Include(t => t.GoodClassShelve)
                .Include(t=>t.Goods)
                .FirstOrDefault(t => t.Section==catName || t.Category == catName || t.ClassName == catName || t.Group==catName || t.Segment==catName);
            return updGoodsClass;
        }

        public static Store GetStore(this CmToolContext cont, string storeCode)
        {
            Store updStore = cont.Stores
                .Include(t => t.StoreShelves)
                .Include(t=>t.StoreSales)
                .FirstOrDefault(t => t.StoreCode == storeCode);
            return updStore;
        }

        public static Good GetGood(this CmToolContext cont, string prodCode)
        {
            Good updGood = cont.Goods.Include(p => p.GoodSales).FirstOrDefault(p => p.Code == prodCode);
            return updGood;
        }
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string GetPropValue(this object src, string propName)
        {
            Type t = src.GetType();
            PropertyInfo p = t.GetProperty(UppercaseFirst(propName));
            string v = (string) p.GetValue(src, null);
            return v;
        }
    }

    public class CmToolRepository : ICmToolRepository
    {
        private CmToolContext _context;

        public CmToolRepository(CmToolContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Good> GetGoodsWithWeeklySales(string catName)  
        {
            var ttGoodsClasses = _context.GoodsClasses.Include(t => t.Goods).ThenInclude(o => o.GoodSales).Where(s => s.Category==catName).ToList();
            IEnumerable<Good> ttGoods = ttGoodsClasses.SelectMany(t => t.Goods).Where(t=>t.GoodSales.Count != 0).ToList();
            return ttGoods.Select(p => new Good()
            {
                Code = p.Code,
                GoodsClass = p.GoodsClass,
                Name = p.Name,
                GoodSales = p.GoodSales.GroupBy(s=>s.Week).Select(z=>new Sale()
                {
                   Salespcs = z.Sum(w=>w.Salespcs),
                   Week = z.Key
                }).ToList()
            }).ToList();
        }

        public IEnumerable<Good> GetSalesTotal(string level, string[] categoryList)
        {
            var result = new List<Good>();
            foreach (string cat in categoryList)
            {

                IEnumerable<GoodsClass> tGoodsClasses = _context.GoodsClasses.Include(t => t.Goods).ThenInclude(o => o.GoodSales).
                    Where(s => s.GetPropValue(level) == cat).ToList();
                IEnumerable<Good> tGoods = tGoodsClasses.SelectMany(t => t.Goods).Select(p => p).ToList();
                ICollection<Sale> totalSalesByWeeks = tGoods.SelectMany(s => s.GoodSales).GroupBy(t => t.Week).Select(z => new Sale()
                {
                    
                    Salespcs = z.Sum(w => w.Salespcs),
                    Week = z.Key
                }).ToList();

                result.Add(new Good
                {
                    Code = "aggregated",
                    Name = cat,
                    GoodSales = totalSalesByWeeks
                });
            }
            return result;
        }
        public void AddShelves(List<Shelve> data)
        {
            foreach (var shelf in data)
            {
                if (_context.GoodsClasses.Any(s => s.Category == shelf.ShelveGoodsClass.Category))
                {
                    if (_context.Stores.Any(s => s.StoreCode == shelf.ShelveStore.StoreCode))
                    {                      
                        var t = _context.Shelves.FirstOrDefault(p=>p.ShelveGoodsClass.Category == shelf.ShelveGoodsClass.Category && p.ShelveStore.StoreCode==shelf.ShelveStore.StoreCode);
                        if (t != null)
                        {
                            t.SectionsNumber = shelf.SectionsNumber;
                            t.ShelvesNumber = shelf.ShelvesNumber;
                            t.ShelvesWidth = shelf.ShelvesWidth;                    
                        }
                        else
                        {
                            _context.GetGoodsClass(shelf.ShelveGoodsClass.Category)?.GoodClassShelve.Add(shelf);
                            _context.GetStore(shelf.ShelveStore.StoreCode)?.StoreShelves.Add(shelf);
                            _context.Shelves.Add(shelf);
                        }
                    }
                    else
                    {
                        _context.GetGoodsClass(shelf.ShelveGoodsClass.Category)?.GoodClassShelve.Add(shelf);                   
                        _context.Stores.Add(shelf.ShelveStore);
                        _context.Shelves.Add(shelf);
                    }
                }
                else
                {
                    _context.GoodsClasses.Add(shelf.ShelveGoodsClass);
             
                    if (_context.Stores.Any(s => s.StoreCode == shelf.ShelveStore.StoreCode))
                    {                     
                        _context.GetGoodsClass(shelf.ShelveGoodsClass.Category)?.GoodClassShelve.Add(shelf);                                         
                        _context.GetStore(shelf.ShelveStore.StoreCode)?.StoreShelves.Add(shelf);
                        _context.Shelves.Add(shelf);
                    }
                    else
                    {
                       // _context.GoodsClasses.Add(shelf.ShelveGoodsClass);
                        _context.Stores.Add(shelf.ShelveStore);
                        _context.Shelves.Add(shelf);
                    }
                }
                _context.SaveChanges();
            }
        }

        public void AddProducts(List<Good> data)
        {
            foreach (var prod in data)
            {
                if (_context.GoodsClasses.Any(s => s.Category == prod.GoodsClass.Category))
                {
                    _context.GetGoodsClass(prod.GoodsClass.Category)?.Goods.Add(prod);      
                    _context.Goods.Add(prod);
                }
                else
                {
                    _context.GoodsClasses.Add(prod.GoodsClass);
                    _context.Goods.Add(prod);
                }
                _context.SaveChanges();
            }
            
        }

        public void AddSales(List<Sale> data)
        {
            var oldGoodCodes = _context.Goods.Select(p => p.Code).ToList();
            var newGoodCodes = data.Select(p => p.ProductCode).ToList().Except(oldGoodCodes).ToList();
            foreach (var sale in data.Where(p=>newGoodCodes.Any(z => z == p.ProductCode)))
            {
                _context.Goods.Add(new Good() {Code = sale.ProductCode});
            }
            var oldStoreCode = _context.Stores.Select(p => p.StoreCode).ToList();
            var newStoreCodes = data.Select(p => p.StoreCode).ToList().Except(oldStoreCode).ToList();
            foreach (var sale in data.Where(p => newStoreCodes.Any(z => z == p.StoreCode)))
            {
                _context.Stores.Add(new Store() { StoreCode = sale.StoreCode});
            }
            _context.SaveChanges();        //    var groupedData = data.GroupBy(p => new  {p.StoreCode, p.ProductCode}).Select(sl=>new List<Sale>());
            var goodList = data.Select(p => p.ProductCode).ToList();
            foreach (var g in goodList)
            {
                var productData = data.Where(p => p.ProductCode == g).ToList();
                var productStoreList = productData.Select(p => p.StoreCode).ToList();
                foreach (var s in productStoreList)
                {
                    var productStoreData = productData.Where(p => p.StoreCode == s).ToList();
                    foreach (var sl in productStoreData)
                    {                       
                        List<Sale> targSales = _context.Sales.Where(p => p.ProductCode == g && p.StoreCode == s).ToList();
                        var existSale = targSales.FirstOrDefault(p => p.Week == sl.Week);
                        if (existSale != null)
                        {
                            existSale.MinStock = sl.MinStock;
                            existSale.Forecast = sl.Forecast;
                            existSale.PurchasingPrice = sl.PurchasingPrice;
                            existSale.SalesPrice = sl.SalesPrice;
                            existSale.Salespcs = sl.Salespcs;
                            existSale.Range = sl.Range;
                        }
                        else
                        {
                            _context.GetGood(g).GoodSales.Add(sl);
                            _context.GetStore(s).StoreSales.Add(sl);
                            targSales.Add(sl);
                        }
                       
                    }
                }
                _context.SaveChanges();
            }
        }

        public void SetGoodsClass(string cName, List<string> goodCodes)
        {
            string catname = cName;
            IEnumerable<Good> toUpdGoods = _context.Goods.Include(c=>c.GoodsClass).Where(o => goodCodes.Any(p => p == o.Code)).ToList();
            foreach (Good good in toUpdGoods)
            {
                GoodsClass existGoodsClass = _context.GoodsClasses.FirstOrDefault(s => s.ClassName == catname && s.Category==good.GoodsClass.Category
                && s.Group==good.GoodsClass.Group && s.Section==good.GoodsClass.Section && s.Segment==good.GoodsClass.Segment);
                if (existGoodsClass == null)
                {
                    var newGoodsClass = new GoodsClass()
                    {
                        Section = good.GoodsClass.Section,
                        Category = good.GoodsClass.Category,
                        Group = good.GoodsClass.Group,
                        Segment = good.GoodsClass.Segment,
                        ClassName = catname
                    };
                    _context.GoodsClasses.Add(newGoodsClass);
                    good.GoodsClass = newGoodsClass;
                    _context.SaveChanges();
                }
                else 
                good.GoodsClass = existGoodsClass;
            }
            _context.SaveChanges();
            var obsoletGoodClasses = _context.GoodsClasses.Include(t=>t.Goods).Where(c => c.Goods.Count == 0 && (c.ClassName != null || c.ClassName != "")).ToList();
            _context.GoodsClasses.RemoveRange(obsoletGoodClasses);
            _context.SaveChanges();
        }

        public void UpdateSaleById(int goodId, int saleId,int salePsc)
        {
            Good updGood = _context.Goods.Include(t => t.GoodSales).FirstOrDefault(t => t.GoodId == goodId);
            Sale updSale =   updGood?.GoodSales.FirstOrDefault(t=>t.SaleId==saleId);
         
            if (updSale != null) updSale.Salespcs = salePsc;
            _context.SaveChanges();
        }

        public void ClearData()
        {
            _context.GoodsClasses.RemoveRange(_context.GoodsClasses);
            _context.Stores.RemoveRange(_context.Stores);
            _context.Shelves.RemoveRange(_context.Shelves);
            _context.Goods.RemoveRange(_context.Goods);
            _context.Sales.RemoveRange(_context.Sales);
            _context.SaveChanges();
        }

        public IEnumerable<GoodsClass> GetGoodsClasses()
        {
            return  _context.GoodsClasses.ToList();
        
         
        }

       
    }
}
