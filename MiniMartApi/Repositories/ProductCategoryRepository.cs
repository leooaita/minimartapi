﻿using Dapper;
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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IConfiguration _config;

        public ProductCategoryRepository(IConfiguration config)
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
        /// Gets the ProductCategory by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ProductCategory> GetByID(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductCategoryFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETBYID");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<ProductCategory>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets all ProductCategory from BD.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategory>> GetAll()
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductCategoryFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "GETALL");
                    var result = await con.QueryAsync<ProductCategory>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ProductCategory> Edit(ProductCategory productCategory)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductCategoryFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", productCategory.Id);
                    param.Add("@Name", productCategory.Name);
                    var result = await con.QueryAsync<ProductCategory>(sQuery, param, commandType: CommandType.StoredProcedure);
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
        public async Task<ProductCategory> Delete(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "ProductCategoryFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<ProductCategory>(sQuery, param, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                if (ex is ConstraintException)
                {
                    throw new Exception("There are associated objects with the Category, you cannot execute this action");
                }
                throw ex;
            }
        }
    }
}

