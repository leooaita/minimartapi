using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMartApi.Models;
using MiniMartApi.Interfaces;
using MiniMartApi.Dtos;
using MiniMartApi.Exceptions;

namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiniMartController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IVoucherRepository _voucherRepository;

        public MiniMartController(IStoreRepository storeRepository,IProductRepository productRepository,ICartRepository cartRepository,IVoucherRepository voucherRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _voucherRepository = voucherRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetAll()
        {
            return await _storeRepository.GetAll();
        }
        [HttpGet("GetStoresBy")]
        public async Task<ActionResult<List<Store>>> GetStoresBy(String storeName, String productName, int hourAvailaBle)
        {
            return await _storeRepository.GetStoresBy(storeName, productName, hourAvailaBle);
        }
        [HttpGet("GetStoresByTime")]
        public async Task<ActionResult<List<Store>>> GetStoresByTime(int isAvailableHour)
        {
            return await _storeRepository.GetStoresByTime(isAvailableHour);
        }
        [HttpGet("GetProductsBy")]
        // Be able to query if a product is available, at a certain store, and return that product's info
        public async Task<ActionResult<List<ResponseProductStockStoreDTO>>> GetProductsBy(String productName,String storeName, int cant)
        {
           return await _productRepository.GetProductAvailableBy(storeName,productName,cant );
        }
        [HttpGet("GetAvailableProductsAcrossStores")]
        // Be able to query all available products, across stores, with their total stock.
        public async Task<ActionResult<List<ResponseProductStockDTO>>> GetAvailableProductsAcrossStores()
        {
            return await _productRepository.GetAvailableProductsAcrossStores();
        }
        /// <summary>
        /// Creates the cart.
        /// </summary>
        /// <param name="StoreId">The store identifier.</param>
        /// <param name="personName">Name of the person.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("CartFor/{Owner}/Store/{StoreId}")]
        public async Task<ActionResult<Cart>> CreateCart(int StoreId, string Owner)
        {
            Cart cart_result = new Cart();
            cart_result.StoreId = StoreId;
            cart_result.Owner = Owner;
            cart_result.Created = DateTime.Now;
            try {
                Store store = await _storeRepository.GetByID(StoreId);
                if (store == null) throw new Exception(String.Format("The store with the id:{0} does not exist in the database", StoreId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return await _cartRepository.Edit(cart_result);
        }
        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="Owner">The owner.</param>
        /// <param name="Cant">The cant.</param>
        /// <returns></returns>
        /// 
        [HttpPost("CartFor/{Owner}/AddProduct/{ProductId}/Cant/{Cant}")]
        public async Task<ActionResult<CartItem>> AddProduct(int ProductId, String Owner, int Cant=1)
        {
            CartItem cartItem = new CartItem();
            try
            {
                IList<Cart> carts = await _cartRepository.GetAll() ;
                Cart cart =carts.FirstOrDefault<Cart>(x =>x.Owner == Owner);
                if (cart == null) throw new Exception(String.Format("The {0}'s cart  does not exist in the database", Owner));
                cartItem.Cant = Cant;
                cartItem.ProductId = ProductId;
                cartItem.CartId = cart.Id;
                return await _cartRepository.EditCartItem(cartItem);
            }
            catch (Exception ex)
            {
                if (ex is MiniMartException)
                {
                    return BadRequest(((MiniMartException)ex).MinimartMessage);
                }                
                return BadRequest(ex.Message);
            }
        }
        //
        [HttpPost("CartFor/{Owner}/DeleteProduct/{ProductId}")]
        public async Task<ActionResult<CartItem>> DeleteProduct(int ProductId, String Owner)
        {
            CartItem cartItem = new CartItem();
            try
            {
                IList<Cart> carts = await _cartRepository.GetAll();
                Cart cart = carts.FirstOrDefault<Cart>(x => x.Owner == Owner);
                if (cart == null) throw new Exception(String.Format("The {0}'s cart  does not exist in the database", Owner));
                cartItem.ProductId = ProductId;
                cartItem.CartId = cart.Id;
            }
            catch (Exception ex)
            {
                if (ex is MiniMartException)
                {
                    BadRequest(((MiniMartException)ex).MinimartMessage);
                } else {
                    return BadRequest(ex.Message);
                }   
            }
            return await _cartRepository.DeleteCartItem(cartItem);
        }

        [HttpPost("CartFor/{Owner}/ApplyVoucher/{VoucherId}/Date/{date_voucher}")]
        public async Task<ActionResult<Cart>> ApplyVoucher(String Owner, String VoucherId, DateTime date_voucher)
        {
            Cart cart;
            try
            {
                IList<Cart> carts = await _cartRepository.GetAll();
                cart = carts.FirstOrDefault<Cart>(x => x.Owner == Owner);
                if (cart == null) throw new Exception(String.Format("The {0}'s cart  does not exist in the database", Owner));

                Voucher voucher = await _voucherRepository.GetByID(VoucherId);
                IList<Tuple<Product, int>> listProducts = new List<Tuple<Product, int>>();
                decimal total = 0;
                foreach (CartItem cartItem in cart.Items)
                {
                    total = total + cartItem.Product.Price * cartItem.Cant;
                    listProducts.Add(cartItem.GetTuple());
                }
                decimal discount = voucher.Calculate(listProducts, date_voucher);
                cart.Total_discount = total;
                cart.Total_discount = total - discount;
                return cart;
            }
            catch (Exception ex)
            {
                if (ex is MiniMartException)
                {
                    return BadRequest(((MiniMartException)ex).MinimartMessage);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            
        }
    }
}
