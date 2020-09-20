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
using MiniMart.Models;
using Microsoft.Extensions.Logging;

namespace MiniMartApi.Repositories
{

    public class VoucherRepository : IVoucherRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public static T GetObject<T>(IDictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                if (type.GetProperty(kv.Key) != null)
                {
                    type.GetProperty(kv.Key).SetValue(obj, kv.Value);
                }
            }
            return (T)obj;
        }

        public static Voucher VoucherConvert(object value)
        {
            var dapperRowProperties = value as IDictionary<string, object>;
            if (dapperRowProperties == null) return null;
            switch (dapperRowProperties["voucherType"])
            {
                case 1:
                    VoucherDiscount result = GetObject<VoucherDiscount>(dapperRowProperties);
                    result.Percent = (int)dapperRowProperties["Percent"];
                    return GetObject<VoucherDiscount>(dapperRowProperties);
                case 2:
                    VoucherDiscountPercentPerUnit result_ = GetObject<VoucherDiscountPercentPerUnit>(dapperRowProperties);
                    result_.Percent = (int)dapperRowProperties["Percent"];
                    result_.applyPerCantUnit = (int)dapperRowProperties["applyPerCantUnit"];
                    return result_;
                case 3:
                    return GetObject<VoucherPayTwoTakeThree>(dapperRowProperties);
                default:
                    return null;
            }
        }
        public VoucherRepository(IConfiguration config, ILogger<CartRepository> logger)
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
        private async Task<List<Voucher>> GetVouchers(string id)
        {
            string sql = MSSqlFunctions.getQueryVoucher(id);
            using (IDbConnection con = Connection)
            {
                con.Open();
                var voucherDictionary = new Dictionary<string, Voucher>();
                var productDictionary = new Dictionary<int, Product>();
                var productCategoryDictionary = new Dictionary<int, ProductCategory>();

                var list =await con.QueryAsync<dynamic, Product, ProductCategory, Voucher>(
                    sql,
                    (voucher_, product, productCategory) =>
                    {
                        Voucher voucherConverted = VoucherRepository.VoucherConvert(voucher_);
                        Voucher voucherEntry;
                        Product productEntry;
                        ProductCategory productCategoryEntry;
                        if (!voucherDictionary.TryGetValue(voucherConverted.Id, out voucherEntry))
                        {
                            voucherEntry = voucherConverted;
                            voucherDictionary.Add(voucherConverted.Id, voucherEntry);
                        }
                        if (product != null)
                        {
                            if (!productDictionary.TryGetValue(product.Id, out productEntry))
                            {
                                productEntry = product;
                                product.productCategory = productCategory;
                                productDictionary.Add(productEntry.Id, productEntry);
                            }
                            voucherEntry.validProducts.Add(product);
                        }
                        if (productCategory != null)
                        {
                            if (!productCategoryDictionary.TryGetValue(productCategory.Id, out productCategoryEntry))
                            {
                                productCategoryEntry = productCategory;
                                productCategoryDictionary.Add(productCategoryEntry.Id, productCategoryEntry);
                            }
                            voucherEntry.validCategorys.Add(productCategory);
                        }
                        return voucherEntry;
                    },
                    splitOn: "Id,Id,Id");
                return list.ToList();
            }
        }
        /// <summary>
        /// Gets the Store by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Voucher> GetByID(string id)
        {
            try
            {
                var list = await GetVouchers(id);
                return list.FirstOrDefault();
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
        public async Task<List<Voucher>> GetAll()
        {
            try
            {
                return await GetVouchers("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<VoucherDiscountPercentPerUnit> Edit(VoucherDiscountPercentPerUnit voucher)
        {
            try
            {
                IEnumerable<int> categorys = voucher.validCategorys.Select<ProductCategory, int>(c => c.Id).ToArray();
                IEnumerable<int> products = voucher.validProducts.Select<Product, int>(p => p.Id).ToArray();
                IEnumerable<int> days = voucher.Days.Select<DayOfWeek, int>(p => (int)p).ToArray();

                using (IDbConnection con = Connection)
                {
                    string sQuery = "VoucherFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "EDIT");
                    param.Add("@Id", voucher.Id);

                    param.Add("@onUpTo", voucher.onUpTo);

                    param.Add("@validCategorys", categorys);

                    param.Add("@validProducts", products);
                    param.Add("@valid_from_day", voucher.valid_from_day);
                    param.Add("@valid_from_month", voucher.valid_from_month);
                    param.Add("@valid_from_year", voucher.valid_from_year);
                    param.Add("@valid_to_day", voucher.valid_to_day);
                    param.Add("@valid_to_month", voucher.valid_to_month);
                    param.Add("@valid_to_year", voucher.valid_to_year);
                    param.Add("@valid_days", days);
                    param.Add("@voucherType", 1);

                    var result = await con.QueryAsync<VoucherDiscountPercentPerUnit>(sQuery, param, commandType: CommandType.StoredProcedure);
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
        public async Task<Voucher> Delete(int id)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = "VoucherFunc";
                    con.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Mode", "DELETE");
                    param.Add("@Id", id);
                    var result = await con.QueryAsync<Voucher>(sQuery, param, commandType: CommandType.StoredProcedure);
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
        public async Task<IList<VoucherStore>> validVouchers(int idStore)
        {
            try
            {
                using (IDbConnection con = Connection)
                {
                    string sQuery = String.Format("select * from Voucher_Store where StoreId = {0}", idStore);
        con.Open();
                    var result = await con.QueryAsync<VoucherStore>(sQuery);
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