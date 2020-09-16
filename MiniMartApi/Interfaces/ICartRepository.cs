using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniMartApi.Models;
namespace MiniMartApi.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByID(int id);
        Task<List<Cart>> GetAll();
        Task<Cart> Edit(Cart store);
        Task<Cart> Delete(int id);
        Task<CartItem> EditCartItem(CartItem cartItem);
        Task<CartItem> DeleteCartItem(CartItem cartItem);
    }
}

