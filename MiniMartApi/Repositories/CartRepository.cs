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
using Microsoft.Extensions.Logging;
using MiniMartApi.Exceptions;

namespace MiniMartApi.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public CartRepository(IConfiguration config,ILogger<CartRepository> logger)
        {
            _config = config;
            _logger = logger;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public IList<Cart> getCarts(Nullable<int> id)
        {
            string sql = MSSqlFunctions.getQueryCart(id);
            using (IDbConnection con = Connection)
            {
                con.Open();
                var cartDictionary = new Dictionary<int, Cart>();
                var cartItemDictionary = new Dictionary<int, CartItem>();
                var productDictionary = new Dictionary<int, Product>();
                var productCategoryDictionary = new Dictionary<int, ProductCategory>();
                var cartVoucherDictionary = new Dictionary<string, Voucher>();

                return con.Query<Cart, CartItem, Product, ProductCategory, dynamic, Cart>(
                    sql,
                    (cart, cartItem, product, productCategory, cartVoucher) =>
                    {
                            Cart cartEntry;
                            CartItem cartItemEntry=null;
                            Product productEntry;
                            ProductCategory productCategoryEntry;

                            if (cartItem != null)
                            {
                                if (!productCategoryDictionary.TryGetValue(productCategory.Id, out productCategoryEntry))
                                {
                                    productCategoryEntry = productCategory;
                                    productCategoryDictionary.Add(productCategoryEntry.Id, productCategoryEntry);
                                }
                                if (!productDictionary.TryGetValue(product.Id, out productEntry))
                                {
                                    productEntry = product;
                                    productDictionary.Add(productEntry.Id, productEntry);
                                }
                                productEntry.productCategory = productCategoryEntry;
                                if (cartItem != null)
                                {
                                    if (!cartItemDictionary.TryGetValue(cartItem.Id, out cartItemEntry))
                                    {
                                        cartItemEntry = cartItem;
                                        cartItemEntry.Id = cartItem.Id;
                                        cartItemEntry.Product = productEntry;
                                        cartItemDictionary.Add(cartItemEntry.Id, cartItemEntry);
                                    }
                                }
                            }
                            Voucher cartVoucher_ = VoucherRepository.VoucherConvert(cartVoucher);
                            Voucher cartVoucherEntry;

                            if (!cartDictionary.TryGetValue(cart.Id, out cartEntry))
                            {
                                cartEntry = cart;
                                cartDictionary.Add(cartEntry.Id, cartEntry);
                            }
                            if (cartVoucher_ != null)
                            {
                                if (!cartVoucherDictionary.TryGetValue(cartVoucher_.Id, out cartVoucherEntry))
                                {
                                    cartVoucherEntry = cartVoucher_;
                                    cartVoucherDictionary.Add(cartVoucherEntry.Id, cartVoucher_);
                                }
                                cartEntry.addVoucher(cartVoucherEntry);
                            }
                            if (cartItemEntry != null) { 
                                cartEntry.Items.Add(cartItemEntry);
                            }
                            return cartEntry;
                    },
                    splitOn: "Id,Id,Id,Id,Id")
                .Distinct()
                .ToList();
            }
        }
        /// <summary>
        /// Gets the Cart by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Cart GetByID(int id)
        {
            try
            {
                return getCarts(id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Gets all store from BD.
        /// </summary>
        /// <returns></returns>
        public List<Cart> GetAll()
        {
            try
            {
                return getCarts(null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<CartItem> EditCartItem(CartItem cartItem)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    int result=0;
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", cartItem.Id);
                    param.Add("@ProductId", cartItem.ProductId);
                    param.Add("@Cant",cartItem.Cant);
                    param.Add("@CartId", cartItem.CartId);
                    param.Add("@result", result, DbType.Int32, ParameterDirection.Output);
                    string sQuery = "CartItemFunc";
                    con.Open();
                    var qry = await con.QueryAsync<CartItem>(sQuery, param, commandType: CommandType.StoredProcedure);
                    if (param.Get<int>("@result") == 5)
                    {
                        throw new MiniMartException("There is not enough stock for the product");
                    }
                    return qry.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<Cart> Edit(Cart cart)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "CartFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", cart.Id);
                    param.Add("@StoreId", cart.StoreId);
                    param.Add("@Created", cart.Created);
                    param.Add("@Owner", cart.Owner);
                    param.Add("@Total", cart.Total);
                    param.Add("@Total_discount", cart.Total_discount);
                    var result = await con.QueryAsync<Cart>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Cart> Delete(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "CartFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<Cart>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<CartItem> DeleteCartItem(CartItem cartItem)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "CartItemFunc";
                    int result = 0;
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@CartId", cartItem.CartId);
                    param.Add("@ProductId", cartItem.ProductId);
                    param.Add("@result", result, DbType.Int32, ParameterDirection.Output);
                    var qry_ = await con.QueryAsync<CartItem>(sQuery, param, commandType: CommandType.StoredProcedure);
                    if (param.Get<int>("@result") == 5)
                    {
                        throw new MiniMartException("There is not enough stock for the product");
                    }
                    return qry_.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                if (ex is ConstraintException)
                {
                    throw new Exception("There are associated objects with the Category, you cannot execute this action");
                }
                throw ex;
            }
        }
    }
}

