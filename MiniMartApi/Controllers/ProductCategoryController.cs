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
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetByID(int id)
        {
            return await _productCategoryRepository.GetByID(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductCategory>>> GetAll()
        {
            return await _productCategoryRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategory>> Edit([FromBody] ProductCategory productCategoryRepository)
        {
            if (productCategoryRepository == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid State");
            }

            return await _productCategoryRepository.Edit(productCategoryRepository);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCategory>> DeleteById(int id)
        {
            return await _productCategoryRepository.Delete(id);
        }
    }

}
