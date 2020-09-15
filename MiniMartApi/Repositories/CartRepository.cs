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
    public class CartRepository : ICartRepository
    {
        private readonly IConfiguration _config;

        public CartRepository(IConfiguration config)
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
        /// Gets the Cart by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Cart> GetByID(int id)
        {
            try
            { 
                string sql = MSSqlFunctions.getQueryCart(id);
                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var cartDictionary = new Dictionary<int, Cart>();
                    var cartItemDictionary = new Dictionary<int, CartItem>();
                    var productDictionary = new Dictionary<int, Product>();
                    var cartVoucherDictionary = new Dictionary<int, CartVoucher>();

                    var list = con.Query<Cart, CartItem, Product, CartVoucher, Cart>(
                        sql,
                        (cart, cartItem, product, cartVoucher) =>
                        {
                            Cart cartEntry;
                            CartItem cartItemEntry;
                            Product productEntry;
                            CartVoucher cartVoucherEntry;

                            if (!cartDictionary.TryGetValue(cart.Id, out cartEntry))
                            {
                                cartEntry = cart;
                                cartDictionary.Add(cartEntry.Id, cartEntry);
                            }

                            if (!cartItemDictionary.TryGetValue(cartItem.Id, out cartItemEntry))
                            {
                                cartItemEntry = cartItem;
                                cartItemDictionary.Add(cartItemEntry.Id, cartItemEntry);
                                cartEntry.Items.Add(cartItemEntry);
                            }

                            return cartEntry;
                        },
                        splitOn: "Id,CartId")
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
        public async Task<List<Cart>> GetAll()
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "CartFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETALL");
                    var result = await con.QueryAsync<Cart>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<CartItem> EditCartItem(CartItem cartItem)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETALL");
                    string sQuery = "CartItemFunc";
                    con.Open();
                    var result = await con.QueryAsync<CartItem>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }
    }
}

