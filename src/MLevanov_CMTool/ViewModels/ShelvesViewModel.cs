using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MLevanov_CMTool.Models;

namespace MLevanov_CMTool.ViewModels
{
    public class ShelvesViewModel
    {
        public int Id { get; set; }
        public string ShelveStore { get; set; }
        public string StoreCode { get; set; }
        public string ShelveGoodsClass { get; set; }
        public int SectionsNumber { get; set; }
        public int ShelvesNumber { get; set; }
        public int ShelvesWidth { get; set; }
    }
}
