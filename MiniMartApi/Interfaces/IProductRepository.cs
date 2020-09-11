using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniMartApi.Models;
namespace MiniMartApi.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByID(int id);
        Task<List<Product>> GetAll();
        Task<Product> Edit(Product store);
        Task<Product> Delete(int id);
    }
}