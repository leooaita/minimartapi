using Dapper;
using Microsoft.Extensions.Configuration;
using MiniMartApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MiniMartApi.Interfaces;
using MiniMartApi.Sqls;

namespace MiniMartApi.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IConfiguration _config;

        public StoreRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        /// <summary>
        /// Gets the Store by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Store> GetByID(int id)
        {
            try
            {

                    string sql = MSSqlFunctions.getQueryStore(id);
                    using (IDbConnection con = Connection)
                    {
                        con.Open();
                        var storeDictionary = new Dictionary<int, Store>();
                        var productDictionary = new Dictionary<int, Product>();
                        var list = con.Query<Store, StockItem, Product, ProductCategory,Store>(
                            sql,
                            (store, stock, product, productCategory) =>
                            {
                                Store storeEntry;
                                Product productEntry;
                                if (!storeDictionary.TryGetValue(store.Id, out storeEntry))
                                {
                                    storeEntry = store;
                                    storeDictionary.Add(storeEntry.Id, storeEntry);
                                }
                                if (product != null)
                                {
                                    if (!productDictionary.TryGetValue(product.Id, out productEntry))
                                    {
                                        productEntry = product;
                                        product.productCategory = productCategory;
                                        productDictionary.Add(productEntry.Id, productEntry);
                                    }
                                    stock.product = product;
                                    storeEntry.Stock.Add(stock);
                                }
                                return storeEntry;
                            },
                            splitOn: "Id")
                        .Distinct()
                        .ToList();

                    return list.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets all store from BD.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Store>> GetAll()
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "StoreFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETALL");
                    var result = await con.QueryAsync<Store>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Store> Edit(Store store)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "StoreFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", store.Id);
                    param.Add("@OpenedFrom", store.OpenedFrom);
                    param.Add("@OpenedTo", store.OpenedTo);
                    param.Add("@Name", store.Name);
                    param.Add("@Address", store.Address);
                    var result = await con.QueryAsync<Store>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Store> Delete(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "StoreFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<Store>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets the stores by time.
        /// Query available stores at a certain time in the day and return only those that
        /// </summary>
        /// <param name="isAvailableHour">The is available hour.</param>
        /// <returns></returns>
        public async Task<List<Store>> GetStoresByTime(int isAvailableHour)
        {
            String query = @"select * from Store ";
            String where = String.Format(" Store.OpenedFrom <= {0} and Store.OpenedTo >= {0} ", isAvailableHour);
            String query_result = String.Format("{0} where {1}", query, where);
            try
            {
                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var result = await con.QueryAsync<Store>(query_result);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
                •
                • Be able to query available products for a particular store
                • Be able to manage a simple virtual cart(add/remove from it). It cannot allow to add a
                product that has NO stock
                • Be able to check the validity of a Voucher code on said virtual cart.Calculate discounts
                and return both original and discounted prices
        */

        public async Task<List<Store>> GetStoresBy(string StoreName, string ProductName, Nullable<int> isAvailableHour)
        {
            IList<String> filter = new List<String>();
            if (StoreName != null)
            {
                filter.Add(String.Format(" Upper(Store.Name) like '%%' ", StoreName.ToUpper()));
            }
            if (ProductName != null)
            {
                filter.Add(String.Format(" Upper(Product.Name) like '%%' ", ProductName.ToUpper()));
            }
            if (isAvailableHour.HasValue)
            {
                filter.Add(String.Format(" Store.OpenedFrom < = {0} and Store.OpenedTo > {0} " , isAvailableHour.Value ));
            } 
            filter.Add(" Stock.Cant > 0 ");

            String where = String.Join(" AND ", filter);
            String query = @"select Store.*,Product.*, ProductCategory.*, Stock.* from Product                                 
                                join ProductCategory on Product.ProductCategoryId = ProductCategory.Id
                                join Stock on Stock.ProductId = Product.Id                                
                                join Store on Store.Id = Stock.StoreId
                            ";
            String query_result = String.Format("{0} where {1}", query, where);
            try
            {
                using (IDbConnection con = Connection)
                {
                    con.Open();

                    IDictionary<int, Store> storeDictionary = new Dictionary<int,Store>();
                    IDictionary<int, Product> productDictionary = new Dictionary<int, Product>();
                    IDictionary<int, ProductCategory> productCategoryRepository = new Dictionary<int, ProductCategory>();

                    var result =  con.Query<Store, StockItem, Product, ProductCategory, Store>(query_result,
                        (store,stock,product,productCategory)=>{
                            
                            Store storeEntry;
                            Product productEntry;
                            if (!storeDictionary.TryGetValue(store.Id, out storeEntry))
                            {
                                storeEntry = store;
                                storeDictionary.Add(storeEntry.Id, storeEntry);
                            }
                            if (product != null)
                            {
                                if (!productDictionary.TryGetValue(product.Id, out productEntry))
                                {
                                    productEntry = product;
                                    product.productCategory = productCategory;
                                    productDictionary.Add(productEntry.Id, productEntry);
                                }
                                stock.product = product;
                                storeEntry.Stock.Add(stock);
                            }
                            
                            return storeEntry;
                        }, splitOn: "Id,Id,Id,Id")
                        .Distinct()
                        .ToList();
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
