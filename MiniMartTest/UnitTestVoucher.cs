using Microsoft.AspNetCore.Routing.Matching;
using MiniMart.Models;
using MiniMartApi.Models;
using MiniMartCommons.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MiniMartTest
{

    public class UnitTestVoucher
    {
        /// <summary>
        /// Gets the mock product category.
        /// </summary>
        /// <param name="typeCategory">The type category.</param>
        /// <returns></returns>
        public ProductCategory getMockProductCategory(ProductCategoryType typeCategory)
        {
            ProductCategory productCategory = new ProductCategory();
            productCategory.Id = (int)typeCategory;
            productCategory.Name = typeCategory.ToString("g");
            return productCategory;
        }
        /// <summary>
        /// The Test output helper
        /// </summary>
        private readonly ITestOutputHelper output;
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestVoucher"/> class.
        /// </summary>
        /// <param name="output">The output Helper</param>
        public UnitTestVoucher(ITestOutputHelper output)
        {
            this.output = output;
        }
        /// <summary>
        /// Gets the mock product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="price">The price.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Product getMockProduct(int id, String name, decimal price, ProductCategoryType typeCategory)
        {
            ProductCategory productCategory = getMockProductCategory(typeCategory);
            Product p = new Product();
            p.Id = id;
            p.Name = name;
            p.productCategory = productCategory;
            p.ProductCategoryId = productCategory.Id;
            p.Price = price;
            return p;
        }
        /// <summary>
        /// Prints the item list.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        public void PrintItemList(IList<Tuple<Product, int>> itemList)
        {
            output.WriteLine("Item List:");
            decimal total = 0;
            foreach (Tuple<Product, int> element in itemList)
            {
                decimal itemTotal = element.Item1.Price * element.Item2;
                total = total + itemTotal;
                output.WriteLine(String.Format("Item:{0}({1})-Cant:{2:000000}-Unit:{3:000000}*****Total:{4:000000}",element.Item1.Name.PadRight(40,'-'), element.Item1.productCategory.Name.PadRight(20, '-'), element.Item2,element.Item1.Price, itemTotal));
            }
            String info = "Total:";
            output.WriteLine(String.Format("{0}{1:000000}", info.PadLeft(100, '-'),total));
        }
        public Voucher GetVoucher(String id)
        {
            Voucher voucherResult;
            switch (id)
            {
                case "COCO1V1F8XOG1MZZ":
                    // 20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
                    voucherResult = new VoucherDiscount(20);
                    voucherResult.Id = id;
                    // Setting id
                    voucherResult.Id = "COCO1V1F8XOG1MZZ";
                    // Valid Vaouchers Week Days
                    voucherResult.Days.Add(DayOfWeek.Wednesday);
                    voucherResult.Days.Add(DayOfWeek.Thursday);
                    // Valid Period
                    voucherResult.valid_from_day = 27;
                    voucherResult.valid_from_month = (int)Month.January;
                    voucherResult.valid_to_day = 13;
                    voucherResult.valid_to_month = (int)Month.February;
                    // Valid Category (Cleaning)
                    voucherResult.validCategorys.Add(getMockProductCategory(ProductCategoryType.Cleaning));
                    break;
                case "COCOKCUD0Z9LUKBN":
                    // Pay 2 take 3 on "Windmill Cookies" on up to 6 units, from Jan 24th to Feb 6th
                    voucherResult = new VoucherPayTwoTakeThree();
                    // Setting id
                    voucherResult.Id = id;
                    // Valid Vaouchers up to 6 units
                    voucherResult.onUpTo = 6;
                    // Valid Period 24 January to 6th February
                    voucherResult.valid_from_day = 24;
                    voucherResult.valid_from_month = (int)Month.January;
                    voucherResult.valid_to_day = 6;
                    voucherResult.valid_to_month = (int)Month.February;
                    break;
                 case "COCOG730CNSG8ZVX":
                    // 10% off on Bathroom and Sodas, from Jan 31th to Feb 9th
                    voucherResult = new VoucherDiscount(10);
                    // Setting id
                    voucherResult.Id = id;
                    // Valid Period
                    voucherResult.valid_from_day = 31;
                    voucherResult.valid_from_month = (int)Month.January;
                    voucherResult.valid_to_day = 9;
                    voucherResult.valid_to_month = (int)Month.February;
                    // Valid Category (Cleaning)
                    voucherResult.validCategorys.Add(getMockProductCategory(ProductCategoryType.Bathroom));
                    voucherResult.validCategorys.Add(getMockProductCategory(ProductCategoryType.Sodas));
                    break;
                case "COCO2O1USLC6QR22":
                    /// 30% off on the second unit (of the same product), on "Nuka-Cola", "Slurm" and "Diet Slurm", for all February
                    voucherResult = new VoucherDiscountPercentPerUnit(0, 30, 2);
                    // Setting id
                    voucherResult.Id = id;
                    // Valid Period
                    voucherResult.valid_from_month = (int)Month.February;
                    voucherResult.valid_to_month = (int)Month.February;
                    // Valid Product: Slurm,Nuke-Cola, Diet Slurm
                    voucherResult.validProducts.Add(getMockProduct(5, "Slurm", 100, ProductCategoryType.Sodas));
                    voucherResult.validProducts.Add(getMockProduct(3, "Nuke-Cola", 100, ProductCategoryType.Sodas));
                    voucherResult.validProducts.Add(getMockProduct(6, "Diet Slurm", 100, ProductCategoryType.Sodas));
                    break;
                case "COCO0FLEQ287CC05":
                    // 50% off on the second unit (of the same product), on "Hang -yourself toothpaste", only on Mondays, first half of February."
                    voucherResult = new VoucherDiscountPercentPerUnit(0, 50, 2);
                    // Setting id
                    voucherResult.Id = id;
                    // Valid Period
                    voucherResult.Days.Add(DayOfWeek.Monday);
                    voucherResult.valid_from_month = (int)Month.February;
                    voucherResult.valid_from_day = 1;
                    voucherResult.valid_to_month = (int)Month.February;
                    voucherResult.valid_to_day = 15;
                    // Valid Product: Hang -yourself toothpaste
                    voucherResult.validProducts.Add(getMockProduct(23, "Hang -yourself toothpaste", 100, ProductCategoryType.Bathroom));
                    break;
                default:
                    throw new Exception("UnitTestVoucher: no result.");
                break;
            }
            return voucherResult;
        }
        /// <summary>
        ///  COCO1V1F8XOG1MZZ: 20% off on Wednesdays and Thursdays, on Cleaning products,from Jan 27th to Feb 13th
        /// </summary>
        /// 
        [Fact]
        public void TestVoucherCOCO1V1F8XOG1MZZ()
        {
            Product cleaningProduct = getMockProduct(15, "Atlantis detergent", 100, ProductCategoryType.Cleaning);
            IList<Tuple<Product, int>> products = new List<Tuple<Product, int>>();
            products.Add(new Tuple<Product, int>(cleaningProduct, 1));
            // 20 % off on Wednesdays and Thursdays, on Cleaning products,from Jan 27th to Feb 13th
            Voucher voucherDiscount = this.GetVoucher("COCO1V1F8XOG1MZZ");
            // In Range
            decimal result_discount = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 28));
            Assert.Equal(20, result_discount);
            // Out of Range 1
            decimal result_discount_zero = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 10));
            Assert.Equal(0, result_discount_zero);
            // Out of Range 2
            decimal result_discount_zero_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 3, 10));
            Assert.Equal(0, result_discount_zero_2);
            // Include 5 units of Generic mop, it cost 40.
            Product cleaningProduct_2 = getMockProduct(2, "Generic mop", 40, ProductCategoryType.Cleaning);
            products.Add(new Tuple<Product, int>(cleaningProduct_2, 5));
            decimal result_discount_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 28));
            Assert.Equal(20 + 40, result_discount_2);
        }
        [Fact]
        /// <summary>
        /// COCOKCUD0Z9LUKBN: Pay 2 take 3 on "Windmill Cookies" on up to 6 units, from Jan 24th to Feb 6th
        /// </summary>
        public void TestVoucherCOCOKCUD0Z9LUKBN()
        {
            IList<Tuple<Product, int>> products = new List<Tuple<Product, int>>();
            Product foodProduct = getMockProduct(8, "Windmill Cookies", 100, ProductCategoryType.Food);
            // Add 7 units of Windmill Cookies on products List
            products.Add(new Tuple<Product, int>(foodProduct, 7));
            // Another product to the list
            Product cleaningProduct = getMockProduct(15, "Atlantis detergent", 100, ProductCategoryType.Cleaning);
            products.Add(new Tuple<Product, int>(cleaningProduct, 1));
            // Create Vouchert pay two take three
            Voucher voucherDiscount = this.GetVoucher("COCOKCUD0Z9LUKBN");
            // In Range
            decimal result_discount = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 28));
            // from 8 units, total cost=800, Voucher valid from up to 6 units,
            // for every 3 units one is free, 3 units = 100 discount 
            // total 7 units=> 3+3+1 = 7 => 100+100+0 => Total discount =200
            Assert.Equal(200, result_discount);
            // Out of Range 1
            decimal result_discount_zero = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 10));
            Assert.Equal(0, result_discount_zero);
            // Out of Range 2
            decimal result_discount_zero_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 3, 10));
            Assert.Equal(0, result_discount_zero_2);
            // Change vouchers config to upto 9  units and in range 
            voucherDiscount.onUpTo = 9;
            decimal result_discount_up_to_9 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 28));
            Assert.Equal(0, result_discount_up_to_9);
        }
        [Fact]
        /// COCOG730CNSG8ZVX: 10% off on Bathroom and Sodas, from Jan 31th to Feb 9th
        public void TestVoucherCOCOG730CNSG8ZVX()
        {
            IList<Tuple<Product, int>> products = new List<Tuple<Product, int>>();

            Product cleaningProduct = getMockProduct(15, "Atlantis detergent", 100, ProductCategoryType.Cleaning);
            // Atlantis Detergent*******Cant:1*****Unit:100*****Total:100
            products.Add(new Tuple<Product, int>(cleaningProduct, 1));
            Product sodaProduct_1 = getMockProduct(3, "Nuke-Cola", 100, ProductCategoryType.Sodas);
            // Nuke-Cola****************Cant:3*****Unit:100*****Total:300
            products.Add(new Tuple<Product, int>(sodaProduct_1, 3));
            Product sodaProduct_2 = getMockProduct(4, "Sprute", 100, ProductCategoryType.Sodas);
            // Sprute*******************Cant:3*****Unit:100*****Total:400
            products.Add(new Tuple<Product, int>(sodaProduct_2, 1));
     
            Product bathroomProduct = getMockProduct(20, "Generic soap", 100, ProductCategoryType.Bathroom);
            
            products.Add(new Tuple<Product, int>(bathroomProduct, 1));

            PrintItemList(products);
            // Voucher discount 10%           
            Voucher voucherDiscount = this.GetVoucher("COCOG730CNSG8ZVX");
            // In Range
            decimal result_discount = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 2, 5));
            Assert.Equal(50, result_discount);
            // Out of Range 1
            decimal result_discount_zero = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 10));
            Assert.Equal(0, result_discount_zero);
            // Out of Range 2
            decimal result_discount_zero_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 3, 10));
            Assert.Equal(0, result_discount_zero_2);
            // Include 5 units of Generic mop, it cost 40.
            Product cleaningProduct_2 = getMockProduct(2, "Generic mop", 40, ProductCategoryType.Cleaning);
            products.Add(new Tuple<Product, int>(cleaningProduct_2, 5));

            decimal result_discount_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 1, 31));
            Assert.Equal(50, result_discount_2);

        }
        [Fact]
        /// COCO2O1USLC6QR22: 30% off on the second unit (of the same product), on "Nuka-Cola", "Slurm" and "Diet Slurm", for all February
        public void TestVoucherCOCO2O1USLC6QR22()
        {
            IList<Tuple<Product, int>> products = new List<Tuple<Product, int>>();
            Product cleaningProduct = getMockProduct(15, "Atlantis detergent", 100, ProductCategoryType.Cleaning);
            // Atlantis Detergent*******Cant:1*****Unit:100*****Total:100
            products.Add(new Tuple<Product, int>(cleaningProduct, 1));
            Product sodaProduct_1 = getMockProduct(3, "Nuke-Cola", 100, ProductCategoryType.Sodas);
            // Nuke-Cola****************Cant:3*****Unit:100*****Total:300
            products.Add(new Tuple<Product, int>(sodaProduct_1, 3));
            Product sodaProduct_2 = getMockProduct(4, "Sprute", 100, ProductCategoryType.Sodas);
            // Sprute*******************Cant:3*****Unit:100*****Total:400
            products.Add(new Tuple<Product, int>(sodaProduct_2, 1));            
            Product bathroomProduct = getMockProduct(20, "Generic soap", 100, ProductCategoryType.Bathroom);
            products.Add(new Tuple<Product, int>(bathroomProduct, 1));
            // Show Item List
            PrintItemList(products);
            // Voucher 30% off on the second unit (of the same product), on "Nuka-Cola","Slurm" and "Diet Slurm", for all February
            Voucher voucherDiscount = this.GetVoucher("COCO2O1USLC6QR22");
            // In Range
            decimal result_discount_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 2,14));
            // Nuke-Colas price is 100, there are 3 units on the list => only one pair
            // the 30% of a unit is  = 30
            Assert.Equal(30, result_discount_2);
            Product slurmSoda = getMockProduct(5, "Slurm", 100, ProductCategoryType.Sodas);
            // Sprute*******************Cant:3*****Unit:100*****Total:300
            products.Add(new Tuple<Product, int>(slurmSoda, 5));
            // Slurm price is 100, there are 5 units on the list => there are 2 pairs
            // the 30% of a two units is  = 60
            decimal result_discount_3 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 2, 14));
            // slurms discount is 60 plus nuke-colas discount is 30 
            Assert.Equal(60+30, result_discount_3);
        }
        [Fact]
        // COCO0FLEQ287CC05: 50% off on the second unit (of the same product), on "Hang-    yourself toothpaste", only on Mondays, first half of February.
        public void TestVoucherCOCO0FLEQ287CC05()
        {
                IList<Tuple<Product, int>> products = new List<Tuple<Product, int>>();
                Product cleaningProduct = getMockProduct(15, "Hang-yourself toothpaste", 100, ProductCategoryType.Cleaning);
                // Atlantis Detergent*******Cant:1*****Unit:100*****Total:100
                products.Add(new Tuple<Product, int>(cleaningProduct, 1));
                Product sodaProduct_1 = getMockProduct(3, "Nuke-Cola", 100, ProductCategoryType.Sodas);
                // Nuke-Cola****************Cant:3*****Unit:100*****Total:300
                products.Add(new Tuple<Product, int>(sodaProduct_1, 3));
                Product sodaProduct_2 = getMockProduct(4, "Sprute", 100, ProductCategoryType.Sodas);
                // Sprute*******************Cant:3*****Unit:100*****Total:400
                products.Add(new Tuple<Product, int>(sodaProduct_2, 1));
                Product slurmSoda = getMockProduct(5, "Slurm", 100, ProductCategoryType.Sodas);
                // Sprute*******************Cant:3*****Unit:100*****Total:300
                products.Add(new Tuple<Product, int>(slurmSoda, 3));
                Product bathroomProduct = getMockProduct(20, "Generic soap", 100, ProductCategoryType.Bathroom);
                products.Add(new Tuple<Product, int>(bathroomProduct, 1));
                products.Add(new Tuple<Product, int>(getMockProduct(23, "Hang -yourself toothpaste", 100, ProductCategoryType.Bathroom),2));
                // Show Item List
                PrintItemList(products);
                // Voucher 50% off on the second unit (of the same product), on "Hang-    yourself toothpaste", only on Mondays, first half of February.
                Voucher voucherDiscount = this.GetVoucher("COCO0FLEQ287CC05");
                // In Range
                decimal result_discount_2 = voucherDiscount.Calculate(products, new DateTime(DateTime.Now.Year, 2, 14));
                // Hang-    yourself toothpaste price is 100
                // the 50% off second unit of yourself toothpaste is  = 50
                Assert.Equal(50, result_discount_2);
        }
    }
}
