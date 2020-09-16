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
using MiniMartApi.Dtos;

namespace MiniMartApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _config;

        public ProductRepository(IConfiguration config)
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
        /// Gets the Product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Product> GetByID(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETBYID");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<Product>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets all Product from BD.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAll()
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETALL");
                    var result = await con.QueryAsync<Product>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Product> Edit(Product product)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", product.Id);
                    param.Add("@Name", product.Name);
                    param.Add("@Price", product.Price);
                    param.Add("@ProductCategoryId", product.ProductCategoryId);
                    var result = await con.QueryAsync<Product>(sQuery, param, commandType: CommandType.StoredProcedure);
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
        public async Task<Product> Delete(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<Product>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets the product available by StoreName and ProductName
        /// Query if a product is available, at a certain store, and return that product's info        
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ProductName">Name of the product.</param>
        /// <param name="cant">The amount of units that request.</param>
        /// <returns>Products</returns>
        public async Task<List<ResponseProductStockStoreDTO>> GetProductAvailableBy(String StoreName, String ProductName, Nullable<int> cant)
        {
            IList<String> filter = new List<String>();
            if (StoreName != null)
            {
                filter.Add(String.Format(" Upper(Store.Name) like '%{0}%' ", StoreName.ToUpper()));
            }
            if (ProductName != null)
            {
                filter.Add(String.Format(" Upper(Product.Name) like '%{0}%' ", ProductName.ToUpper()));
            }
            if (cant.HasValue)
            {
                filter.Add(String.Format(" Stock.Cant >= {0}", cant.Value));
            }
            String where = String.Join(" AND ", filter);
            String query = @"select Product.*, ProductCategory.*,Store.*,Stock.* from Product                                 
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
                    IDictionary<int, ResponseProductStockStoreDTO> storeDictionary = new Dictionary<int, ResponseProductStockStoreDTO>();
                    var result = con.Query<Product,ProductCategory,Store,StockItem, ResponseProductStockStoreDTO>(query_result,
                        (product,productCategory,store,stockItem) =>
                        {
                            ResponseProductStockStoreDTO responseEntry;
                            if (!storeDictionary.TryGetValue(stockItem.Id, out responseEntry))
                            {
                                responseEntry = new ResponseProductStockStoreDTO();
                                responseEntry.product = product;
                                responseEntry.store = store;
                                responseEntry.cant = stockItem.Cant;
                                storeDictionary.Add(stockItem.Id, responseEntry);
                            }
                            return responseEntry;
                        }, splitOn: "Id"
                        )    
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

        /// <summary>
        /// Gets the available products across stores.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResponseProductStockDTO>> GetAvailableProductsAcrossStores()
        {
            String query = @"
                        select Total ,Product.*, ProductCategory.* from Product 
                        join ProductCategory on ProductCategory.Id = Product.ProductCategoryId 
                        join (select Product.Id, sum(Stock.Cant) as Total from Product join Stock on Product.Id = Stock.ProductId group by Product.Id) total
                        on total.id = Product.Id order by ProductCategory.Name, Product.Name
                        ";
            try
            {
                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var result = con.Query<int,Product, ProductCategory,ResponseProductStockDTO>(query,
                        (total, product, productCategory) =>
                        {
                            ResponseProductStockDTO responseEntry = new ResponseProductStockDTO();
                            responseEntry.product = product;
                            responseEntry.product.productCategory = productCategory;
                            responseEntry.Total = total;
                            return responseEntry;
                        }, splitOn: "Id"
                        )

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


