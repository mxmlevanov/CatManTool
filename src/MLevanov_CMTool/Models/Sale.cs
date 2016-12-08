using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLevanov_CMTool.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string Week { get; set; }
        public int MinStock { get; set; }
        public int StartStock { get; set; }
        public string ProductCode { get; set; }
        public int Salespcs { get; set; }
        public int Forecast { get; set; }
        public float SalesPrice { get; set; }
        public float PurchasingPrice { get; set; }
        public string StoreCode { get; set; }
        public int Range { get; set; }
        public Good SalesGood { get; set; }
        public Store SalesStore { get; set; }
    }

    public class Good
    {
        public int GoodId { get; set; }
        public string Code { get; set; }
        public GoodsClass GoodsClass { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public int MinPack { get; set; }
        public ICollection<Sale> GoodSales { get; set; }
    }
    public class GoodsClass
    {
        public int GoodsClassId { get; set; }
        public string Section { get; set; }
        public string Category { get; set; }
        public string Group { get; set; }
        public string Segment { get; set; }
        public string ClassName { get; set; }
        public ICollection<Shelve>  GoodClassShelve { get; set; }
        public ICollection<Good> Goods { get; set; }
    }

    public class Shelve
    {
        public int ShelveId { get; set; }
        public Store ShelveStore { get; set; }
        public GoodsClass ShelveGoodsClass { get; set; }
        public float ShelvesRate { get; set; }
        public int SectionsNumber { get; set; }
        public int ShelvesNumber { get; set; }
        public int ShelvesWidth { get; set; }
    }

    public class Store
    {
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public float TotalLength { get; set; }
        public float StoreRate { get; set; }
        public ICollection<Shelve> StoreShelves { get; set; }
        public ICollection<Sale> StoreSales { get; set; }
    }


}

