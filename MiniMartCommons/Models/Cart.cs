using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace MiniMartApi.Models
{
    public class Cart : IEquatable<Cart>
    {
        public Cart()
        {
            this.Items = new List<CartItem>();
            this.Vouchers = new List<Voucher>();
        }
        public Int32 Id { get; set; }
        public Int32 StoreId { get; set; }
        public string Owner { get; set; }
        public DateTime Created { get; set; }
        public IList<CartItem> Items { get;set;}
        public IList<Voucher> Vouchers { get; set; }
        public decimal Total { get; set; }
        public decimal Total_discount { get; set; }

        public bool Equals(Cart other)
        {
            return (this.Id == other.Id);
        }
    }
}
