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
                using (IDbConnection con = Connection)
                {
                    string sQuery = "StoreFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETBYID");
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
    }
}
