using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMartApi.Models;
using MiniMartApi.Interfaces;

namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetByID(int id)
        {
            return await _cartRepository.GetByID(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Cart>>> GetAll()
        {
            return await _cartRepository.GetAll();
        }

        [HttpPost]
        [Route("{cartId}/AddProduct/{productId}/{cant}")]
        public async Task<ActionResult<CartItem>> Edit( int cartId,int productId,int cant)
        {
            CartItem cartItem = new CartItem();
            cartItem.CartId = cartId;
            cartItem.ProductId = productId;
            cartItem.Cant = cant;
            return await _cartRepository.EditCartItem(cartItem);
        }
        [HttpPost]
        [Route("{cartId}/DeleteProduct/{productId}")]
        public async Task<ActionResult<CartItem>> Delete(int cartId, int productId)
        {
            CartItem cartItem = new CartItem();
            cartItem.CartId = cartId;
            cartItem.ProductId = productId;
            return await _cartRepository.DeleteCartItem(cartItem);
        }


        [HttpPost]
        public async Task<ActionResult<Cart>> Edit([FromBody] Cart cart)
        {
            if (cart == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid State");
            }

            return await _cartRepository.Edit(cart);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteById(int id)
        {
            return await _cartRepository.Delete(id);
        }
    }

}
