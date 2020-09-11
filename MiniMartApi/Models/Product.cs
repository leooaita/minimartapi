using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Models
{
    public class Product
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Int32 ProductCategoryId { get; set; }
    }
}
