using MiniMartApi.Models;
using MiniMartCommons.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace MiniMartTest
{

    public class UnitTestVoucher
    {
        /// <summary>
        ///  COCO1V1F8XOG1MZZ: 20% off on Wednesdays and Thursdays, on Cleaning products,from Jan 27th to Feb 13th
        /// </summary>
        [Fact]
        public void TestVoucherCOCO1V1F8XOG1MZZ()
        {
            ProductCategory cleaningProductCategory = new ProductCategory();
            cleaningProductCategory.Name = "Cleaning";
            cleaningProductCategory.Id = (int)ProductCategoryType.Cleaning;

            Product cleaningProduct = new Product();
            cleaningProduct.productCategory = cleaningProductCategory;
            cleaningProduct.Price = 100;
            cleaningProduct.Name = "Atlantis detergent";

            IList<Tuple<Product,int>> products = new List<Tuple<Product,int>>();
            products.Add(new Tuple<Product,int>(cleaningProduct,1));
            // 20 % off on Wednesdays and Thursdays, on Cleaning products,from Jan 27th to Feb 13th
            VoucherDiscount voucherDiscount = new VoucherDiscount(20);
            // Setting id
            voucherDiscount.Id="COCO1V1F8XOG1MZZ";
            // Valid Vaouchers Week Days
            voucherDiscount.Days.Add(DayOfWeek.Wednesday);
            voucherDiscount.Days.Add(DayOfWeek.Thursday);
            // Valid Period
            voucherDiscount.valid_from_day = 27;
            voucherDiscount.valid_from_month=(int)Month.January;
            voucherDiscount.valid_to_day = 13;
            voucherDiscount.valid_to_month = (int)Month.February;
            // Valid Category
            voucherDiscount.validCategorys.Add(cleaningProductCategory);

            // In Range
            decimal result_discount = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year,1, 28));
            Assert.Equal(20,result_discount);
            // Out of Range 1
            decimal result_discount_zero = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 10));
            Assert.Equal(0, result_discount_zero);
            // Out of Range 2
            decimal result_discount_zero_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 3, 10));
            Assert.Equal(0, result_discount_zero_2);

            // Include 5 units of Generic mop, it cost 40.
            Product cleaningProduct_2 = new Product();
            cleaningProduct_2.Id = 2;
            cleaningProduct_2.productCategory = cleaningProductCategory;
            cleaningProduct_2.Price = 40;
            cleaningProduct_2.Name = "Generic mop";
            products.Add(new Tuple<Product, int>(cleaningProduct_2, 5));

            decimal result_discount_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 28));
            Assert.Equal(20+40, result_discount_2);

        }
    }
}
