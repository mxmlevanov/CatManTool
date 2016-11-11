using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLevanov_CMTool.Models;

namespace MLevanov_CMTool.ViewModels
{
    public class GoodViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public ICollection<SaleViewModel> GoodSales { get; set; }
        public string Name { get; set; }
        public GoodsClassViewModel GoodsClass { get; set; }
       
    }
}
