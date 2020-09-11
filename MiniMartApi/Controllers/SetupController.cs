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
                var result = await connection.QueryAsync<String>(MSSqlLayer.getCreateStore());
                List<String> log_results = result.ToList();
                try
                {
                    var resultFunction = await connection.QueryAsync<String>(MSSqlLayer.getFunctionStore());
                    log_results.Add("Store function created");
                } catch (SqlException e)
                {
                    log_results.Add("Error creating store function");
                }
                return log_results;
            }
        }
    }
}
