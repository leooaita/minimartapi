using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Models
{
    public interface IStoreRepository
    {
        Task<Store> GetByID(int id);
        Task<List<Store>> GetAll();
        Task<Store> Edit(Store store);
        Task<Store> Delete(int id);
    }
}
