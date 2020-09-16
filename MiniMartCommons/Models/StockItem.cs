using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMartApi.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
		public int Cant { get; set; }
        public int StoreId { get; set; }
        public Product product { get; set; }
    }
}
