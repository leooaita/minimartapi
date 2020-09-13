using System;
using System.Collections.Generic;

namespace MiniMartApi.Models
{
    public enum VoucherType
    {
        Simple = 1,
        PayTwoTakeThree = 2,
        DiscountOnSecondUnit= 3
    }
    public abstract class Voucher
    {
        public Voucher()
        {
            this.Days = new List<DayOfWeek>();
            this.validCategorys = new List<ProductCategory>();
            this.validProducts = new List<Product>();
        }
        public String Id { get; set; }
        public IList<ProductCategory> validCategorys { get; set; }
        public IList<Product> validProducts { get; set; }
        public int onUpTo { get; set; }
        public IList<DayOfWeek> Days { get; set; }
        public int? valid_from_day { get; set; }
        public int? valid_from_month { get; set; }
        public int? valid_from_year { get; set; }
        public int? valid_to_day { get; set; }
        public int? valid_to_month { get; set; }
        public int? valid_to_year { get; set; }
        /// <summary>
        /// Inner Class that defines behavior like the 3x2 calculation per item and quantity
        /// </summary>
        public class ItemCantTotal {
            public Product product { get; set; }         
            public int cant { get; set; }
            public int upTo { get; set; }
            public int percent { get; set; }
            public int perApplyPerCantUnit { get; set; }
            public ItemCantTotal(Product product, int cant, int upTo=0, int percent = 100, int perApplyPerCantUnit = 3)
            {
                this.product= product;
                this.cant = cant;
                this.upTo = upTo;
                this.perApplyPerCantUnit = perApplyPerCantUnit;
                this.percent = percent;
            }
            public decimal TotalItemDiscount() {
                if (upTo < cant)
                {
                    decimal rewarded = product.Price * percent/ 100;
                    return Decimal.ToInt32(cant / perApplyPerCantUnit) * rewarded;
                }
                return 0;
            }
        }
        /// <summary>
        /// Determines whether a date is within a Voucher's date range
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <c>true</c> if [is valid date] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public bool isValidDate(DateTime date)
        {
            int _from_day = valid_from_day.HasValue ? valid_from_day.Value : 1;
            int _from_month = valid_from_month.HasValue ? valid_from_month.Value : 1;
            int _from_year = valid_from_year.HasValue ? valid_from_year.Value : date.Year;
            int _to_month = valid_to_month.HasValue ? valid_to_month.Value : date.Month;

            int _to_year = valid_to_year.HasValue ? valid_to_year.Value : date.Year;
            if (!valid_to_year.HasValue && valid_to_month.HasValue)
            {
                if (valid_to_month.Value < _from_month)
                {
                    _to_year = _to_year + 1;
                }
            }
            // Calculte to Day, considering leap year
            // First day of next month minus one day = last day of current month
            int _to_day = valid_to_day.HasValue ? valid_to_day.Value : (new DateTime(_to_year, _to_month+1,1).AddDays(-1)).Day;

            DateTime _from = new DateTime(_from_year, _from_month, _from_day);
            DateTime _to = new DateTime(_to_year, _to_month, _to_day);

            return _from <= date && _to>= date;
        }
        public bool isValidProduct(Product p)
        {
            return (this.validProducts.Count == 0)?true:this.validProducts.Contains(p);
        }
        public bool isValidProductCategory(ProductCategory productCategory)
        {
            return (this.validCategorys.Count == 0)?true:this.validCategorys.Contains(productCategory);            
        }
        public abstract decimal Calculate(IList<Tuple<Product, int>> products, DateTime date);
    }
}
