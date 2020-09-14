using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniMart.Models;
using MiniMartApi.Models;
namespace MiniMartApi.Interfaces
{
    public interface IVoucherRepository
    {
        Task<Voucher> GetByID(string id);
        Task<List<Voucher>> GetAll();
        Task<VoucherDiscountPercentPerUnit> Edit(VoucherDiscountPercentPerUnit voucher);
        Task<Voucher> Delete(int id);
    }
}