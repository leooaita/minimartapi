using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniMartApi.Models;
namespace MiniMartApi.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> GetByID(int id);
        Task<List<ProductCategory>> GetAll();
        Task<ProductCategory> Edit(ProductCategory store);
        Task<ProductCategory> Delete(int id);
    }
}