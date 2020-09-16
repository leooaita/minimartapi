using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniMartApi.Models;
namespace MiniMartApi.Interfaces
{
    public interface IStoreRepository
    {
        Task<Store> GetByID(int id);
        Task<List<Store>> GetAll();
        Task<Store> Edit(Store store);
        Task<Store> Delete(int id);

        /// Query available stores at a certain time in the day and return only those that
        Task<List<Store>> GetStoresByTime(int isAvailableHour);
        Task<List<Store>> GetStoresBy(String StoreName, String ProductName, Nullable<int> isAvailableHour);
    }
}
