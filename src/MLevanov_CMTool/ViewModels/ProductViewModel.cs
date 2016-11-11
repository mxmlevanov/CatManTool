using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLevanov_CMTool.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string GoodsClass { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public int MinPack { get; set; }
    }
}
