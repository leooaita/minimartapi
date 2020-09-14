using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniMartApi.Sqls;
namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SetupController(IConfiguration config)
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
        /// Setups Database tables and functions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<String>>> Setup()
        {
            using (var connection = this.Connection)
            {
                connection.Open();
                // Store Entity
                var result = await connection.QueryAsync<String>(MSSqlFunctions.getCreateStore());
                List<String> log_results = result.ToList();
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlFunctions.getFunctionStore());
                    log_results.Add("Store function created");
                } catch (SqlException e)
                {
                    log_results.Add("Error creating store function");
                }
                // Product Category Entity
                var resultProductCategory = await connection.QueryAsync<String>(MSSqlFunctions.getCreateProductCategory());
                log_results =log_results.Union(resultProductCategory.ToList()).ToList();
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlFunctions.getFunctionProductCategory());
                    log_results.Add("Product Category function created");
                }
                catch (SqlException e)
                {
                    log_results.Add("Error creating Product Category function");
                }
                // Product Entity
                var resultProduct = await connection.QueryAsync<String>(MSSqlFunctions.getCreateProduct());
                log_results = log_results.Union(resultProduct.ToList()).ToList();
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlFunctions.getFunctionProduct());
                    log_results.Add("Product function created");
                }
                catch (SqlException e)
                {
                    log_results.Add("Error creating Product function");
                }

                // Voucher Entity
                var resultVoucher = await connection.QueryAsync<String>(MSSqlFunctions.getCreateVoucher());
                log_results = log_results.Union(resultVoucher.ToList()).ToList();
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlFunctions.getFunctionVoucher());
                    log_results.Add("Voucher function created");
                }
                catch (SqlException e)
                {
                    log_results.Add("Error creating Voucher function");
                }
                // Stock Entity
                var resultStock = await connection.QueryAsync<String>(MSSqlFunctions.getCreateStock());
                log_results = log_results.Union(resultStock.ToList()).ToList();
                // Constraint
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlFunctions.getConstraint());
                    log_results.Add("Constraints created");
                }
                catch (SqlException e)
                {
                    log_results.Add("Error creating Constraint");
                }
                return log_results;
            }
        }
    }
}
