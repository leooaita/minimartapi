using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Models
{
    public class ProductCategory : IEquatable<ProductCategory>
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public bool Equals(ProductCategory other)
        {
            return (this.Id == other.Id);
        }
    }
}
