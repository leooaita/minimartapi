using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Models
{
    public class Product : IEquatable<Product>
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Int32 ProductCategoryId { get; set; }
        public ProductCategory productCategory { get; set; }

        public bool Equals(Product other)
        {
            return (this.Id == other.Id);
        }
    }
}
