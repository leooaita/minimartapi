using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace MiniMartApi.Models
{
    public class CartItem : IEquatable<CartItem>
    {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public Product Product { get; set; }
            public int Cant { get; set; }
            public int CartId { get; set; }
            public decimal Total { get; }
            public decimal Total_discount { get; }
            public bool Equals(CartItem other)
            {
                return (this.Id == other.Id);
            }
    }   
}
