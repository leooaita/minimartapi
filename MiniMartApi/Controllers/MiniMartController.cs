using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMartApi.Models;
using MiniMartApi.Interfaces;
using MiniMartApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiniMartController : Controller
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public MiniMartController(IStoreRepository storeRepository,IProductRepository productRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
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

    }

}
