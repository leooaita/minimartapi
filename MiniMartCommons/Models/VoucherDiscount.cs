using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMart.Models
{
    public class VoucherDiscount : Voucher
    {
        private int Percent { get; set; }
        public VoucherDiscount(int pecent) : base()
        {
            this.Percent = pecent;
        }
        public override decimal Calculate(IList<Tuple<Product, int>> products, DateTime date)
        {
            decimal result_discount = 0;

            if (this.isValidDate(date))
            {
                foreach (Tuple<Product, int> p in products)
                {
                    if (isValidProduct(p.Item1) && isValidProductCategory(p.Item1.productCategory))
                    {
                        result_discount = result_discount + (p.Item1.Price * p.Item2) * ((decimal)this.Percent / 100);
                    }
                }
            }
            return result_discount;
        }
    }
}
