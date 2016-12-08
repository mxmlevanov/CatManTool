using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLevanov_CMTool.Models;

namespace MLevanov_CMTool.ViewModels
{
    public class SaleViewModel
    {
      
       
      
        public int Id { get; set; }
        public string Week { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Salespcs { get; set; }
        public float SalesPrice { get; set; }
        public float PurchasingPrice { get; set; }
        public string StoreCode { get; set; }
        public int Forecast { get; set; }
        public int Range { get; set; }



    }
}
