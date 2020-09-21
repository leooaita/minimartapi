using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MiniMartApi;
using Xunit;
using System.Collections.Generic;
using MiniMartApi.Models;
using System.Linq;
using MiniMartApi.Dtos;
using System.Security.Policy;
using System.Buffers;
using MiniMartTest.Commons;

namespace MiniMartTest
{

    public class TestIntegractionMiniMartApi : IClassFixture<TestVoucherEnvironment<Startup>>
    {
        private HttpClient Client;

        public TestIntegractionMiniMartApi(TestVoucherEnvironment<Startup> env)
        {
            Client = env.Client;
        }
        public async Task<IList<T>> getAll<T>(String request)
        {
            var response = await Client.GetAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();
            String responseStr = await response.Content.ReadAsStringAsync();
            List<T> objectList = JsonConvert.DeserializeObject<List<T>>(responseStr);
            return objectList;
        }
        [Fact]
        public async Task TestGetMiniMartStores()
        {
            IList<Store> storeList = await getAll<Store>("/api/Store");
            // Get Stores
            Assert.True(storeList.Count != 0);
        }
    // Be able to query available stores at a certain time in the day and return only those that apply
    [Fact]
        public async Task TestGetMiniMartStoresByTime()
        {
            // Gets All Stores
            var request = "/api/Minimart";
            var response = await Client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            String responseStr = await response.Content.ReadAsStringAsync();
            List<Store> storeList = JsonConvert.DeserializeObject<List<Store>>(responseStr);
            // Get max Open hour
            int count_stores = 0;
            if (storeList.Count > 0)
            {
                int maxFromHour = 0;
                foreach (Store s in storeList)
                {
                    maxFromHour = s.OpenedFrom > maxFromHour ? s.OpenedFrom : maxFromHour;
                }
                // count of stores that are open at maxFromHour 
                count_stores = storeList.Where(x => x.OpenedFrom <= maxFromHour && x.OpenedTo >= maxFromHour).Count();
                // Call the Api method that responses stores opened at maxFromHour
                var requestStoresByTime = String.Format("/api/MiniMart/GetStoresByTime?isAvailableHour={0}", maxFromHour);
                var responseStoresByTime = await Client.GetAsync(requestStoresByTime);
                // Assert
                response.EnsureSuccessStatusCode();
                String responseStoresOpenedAtTimeStr = await responseStoresByTime.Content.ReadAsStringAsync();
                List<Store> storeListOpenedAtTime = JsonConvert.DeserializeObject<List<Store>>(responseStoresOpenedAtTimeStr);
                // Must by the same: query by time at maxFromHour with the result of the first query with all stores filtered by time
                Assert.True(storeListOpenedAtTime.Count == count_stores);
            }
        }
        public async void DeleteCart(int Id)
        {
            var request = String.Format("/api/Cart/{0}", Id);
            var response = await Client.DeleteAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();
        }
        public async Task<Cart> CreateCart(String storename, String Owner)
        {
            // Delete if exists
            IList<Cart> carts = await getAll<Cart>("/api/Cart");
            Cart cart_exists = carts.Where(c => c.Owner.Equals(Owner)).FirstOrDefault(); 
            if (cart_exists!=null)
            {
                DeleteCart(cart_exists.Id);
            }
            IList<Store> storeList = await getAll<Store>("/api/Minimart");
            Store store = storeList.Where(s => s.Name.Equals(storename)).FirstOrDefault();
            Assert.True(store != null);
            var request = String.Format("/api/MiniMart/CartFor/{0}/Store/{1}",Owner, store.Id);
            var response = await Client.PostAsync(request,null);
            // Assert
            response.EnsureSuccessStatusCode();
            String responseStr = await response.Content.ReadAsStringAsync();
            Cart cart = JsonConvert.DeserializeObject<Cart>(responseStr);
            return cart;
        }
        /// <summary>
        /// Add product cant to Cart.
        /// </summary>
        /// <param name="Owner">The user defined tag, used to identify a cart</param>
        /// <param name="Product">The name of the product</param>
        /// <param name="count">Amount of units to add</param>
        /// <returns></returns>
        public async Task<Cart> CartAddProductCant(String Owner,String Product, int count)
        {
            IList<Cart> cartList = await getAll<Cart>("/api/Cart");
            Cart cart = cartList.Where(s => s.Owner.Equals(Owner)).FirstOrDefault();
            Assert.True(cart != null,String.Format("Cart for Owner {0} doesn't exists.",Owner));

            IList<Product> productList = await getAll<Product>("/api/Product");
            Product product_ = productList.Where(s => s.Name.Equals(Product)).FirstOrDefault();
            Assert.True(product_ != null, String.Format("Product {0} doesn't exists.",Product));

            var request = String.Format("api/MiniMart/CartFor/{0}/AddProduct/{1}/Cant/{2}", Owner, product_.Id,count);
            var response = await Client.PostAsync(request, null);
            // Assert
            response.EnsureSuccessStatusCode();
            //String responseStr = await response.Content.ReadAsStringAsync();
            //Cart cart = JsonConvert.DeserializeObject<Cart>(responseStr);
            return cart;
        }
        /// <summary>
        /// Tests the voucher COCO1V1F8XOG1MZZ
        /// 20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
        /// With Virulanita
        /// </summary>
        [Fact]
        public async Task TestVoucher()
        {
            String Voucher_ID = "COCO1V1F8XOG1MZZ";
            String UserCartTag = "Rogelio_Coco_Bay";
            String StringDate = "1-30-2020"; // was Wednesday
            // TestVoucher COCO1V1F8XOG1MZZ
            // 1 Create a Cart asociated to Coco Bay for tag "Rogelio_Coco_Bay", if exists then delete cart and create again
            Cart cart = await CreateCart("COCO Bay", UserCartTag);
            Assert.True(cart != null);
            // 2 Adding two units of virulanita (56.18 per unit) to the Rogelio's cart
            Cart cart_2 = await CartAddProductCant("Rogelio_Coco_Bay", "Virulanita", 2);
            // 3 Aplying discount voucher
            String request = String.Format("/api/MiniMart/CartFor/{0}/ApplyVoucher/{1}/Date/{2}", UserCartTag, Voucher_ID,StringDate);
            var response_cart_discount = await Client.PostAsync(request, null);
            String responseStr = await response_cart_discount.Content.ReadAsStringAsync();
            Cart cart_ = JsonConvert.DeserializeObject<Cart>(responseStr);
            decimal total_discount = (decimal)((56.18 * 2) * 0.80);
            Assert.True(cart_.Total_discount == total_discount);
            // Assert
            response_cart_discount.EnsureSuccessStatusCode();
        }
    }
}
