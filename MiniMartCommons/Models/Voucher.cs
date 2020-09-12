using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public int? valid_from_day {get;set;}
        public int? valid_from_month { get;set;}
        public int? valid_from_year { get; set; }
        public int? valid_to_day { get; set; }
        public int? valid_to_month { get; set; }
        public int? valid_to_year { get; set; }

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
            
            int _to_day = valid_to_day.HasValue ? valid_to_day.Value : (new DateTime(_to_year, _to_month+1,1).AddDays(-1)).Day;

            DateTime _from = new DateTime(_from_year, _from_month, _from_day);
            DateTime _to = new DateTime(_to_year, _to_month, _to_day);

            return _from <= date && _to>= date;

        }

        public bool isValidProduct(Product p)
        {
            if (this.validProducts.Count != 0)
            {
                this.validProducts.Contains(p);
            }
            return true;
        }
        public bool isValidProductCategory(ProductCategory productCategory)
        {
            if (this.validCategorys.Count != 0)
            {
                this.validCategorys.Contains(productCategory);
            }
            return true;
        }
        public abstract decimal Calculate(IList<Tuple<Product, int>> products, DateTime date);

    }
    /*
   COCO Bay has:
COCO1V1F8XOG1MZZ: 20% off on Wednesdays and Thursdays, on Cleaning products,
from Jan 27th to Feb 13th
COCOKCUD0Z9LUKBN: Pay 2 take 3 on "Windmill Cookies" on up to 6 units, from Jan 24th
to Feb 6th

COCO Mall has:
COCOG730CNSG8ZVX: 10% off on Bathroom and Sodas, from Jan 31th to Feb 9th

COCO Downtown has:
COCO2O1USLC6QR22: 30% off on the second unit (of the same product), on "Nuka-Cola",
"Slurm" and "Diet Slurm", for all February

COCO0FLEQ287CC05: 50% off on the second unit (of the same product), on "Hang-
yourself toothpaste", only on Mondays, first half of February.
     * */
    public class VoucherPayTwoTakeThree : Voucher
    {
        public override decimal Calculate(IList<Tuple<Product, int>> products, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
    public class VoucherDiscount : Voucher
    {
        private int Percent { get; set; }
        public VoucherDiscount(int pecent) : base()
        {
            this.Percent = pecent;
        }

        public override decimal Calculate(IList<Tuple<Product, int>> products,DateTime date)
        {
            decimal result_discount= 0;
            
            if (this.isValidDate(date))
            {
                foreach(Tuple<Product, int> p in products)
                {
                    if (isValidProduct(p.Item1) && isValidProductCategory(p.Item1.productCategory))
                    {
                        result_discount = result_discount + (p.Item1.Price * p.Item2) * ((decimal)this.Percent/100);
                        
                    }
                }
            }
            return result_discount;
        }
    }
    public class VoucherDiscountOnSecondUnit : Voucher
    {
        public override decimal Calculate(IList<Tuple<Product, int>> products, DateTime date)
        {
            throw new NotImplementedException();
        }
    }



}
