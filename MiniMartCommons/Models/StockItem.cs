using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMartApi.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public Product product { get; set; }
		public int cant { get; set; }
        public int storeId { get; set; }
    }
}
