using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Models
{
    public class Store
    {
        public Int32 Id { get; set; }
        public int OpenedFrom { get; set; }
        public int OpenedTo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IList<StockItem> Stock { get; set; } 

        public Store():base()
        {
            this.Stock = new List<StockItem>();
        }
    }
}
