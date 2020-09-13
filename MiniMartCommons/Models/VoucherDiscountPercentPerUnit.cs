using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMart.Models
{
    public class VoucherDiscountPercentPerUnit : Voucher
    {
        public int percent { get; set; }
        public int applyPerCantUnit { get; set; }

        public VoucherDiscountPercentPerUnit(int onUpTo, int percent, int applyPerCantUnit) :base()
        {
            this.onUpTo = onUpTo;
            this.percent = percent;
            this.applyPerCantUnit = applyPerCantUnit;
        }
        public override decimal Calculate(IList<Tuple<Product, int>> products, DateTime date)
        {
            IList<Tuple<Product, int>> product_candidates = new List<Tuple<Product, int>>();
            if (this.isValidDate(date))
            {
                foreach (Tuple<Product, int> p in products)
                {
                    if (isValidProduct(p.Item1) && isValidProductCategory(p.Item1.productCategory))
                    {
                        product_candidates.Add(p);
                    }
                }
            }
            IList<ItemCantTotal> product_candidates_total =
                product_candidates.GroupBy(d => d.Item1).Select(
                    g => new ItemCantTotal(g.Key, g.Sum(s => s.Item2), this.onUpTo,this.percent,this.applyPerCantUnit)
                ).ToList();
            // Total ItemDoscount Discount per total cant item 
            return product_candidates_total.Sum(item => item.TotalItemDiscount());
        }
    }
}
