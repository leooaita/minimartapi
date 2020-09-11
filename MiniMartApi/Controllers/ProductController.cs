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
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetByID(int id)
        {
            return await _productRepository.GetByID(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Edit([FromBody] Product product)
        {
            if (product  == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid State");
            }

            return await _productRepository.Edit(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteById(int id)
        {
            return await _productRepository.Delete(id);
        }
    }

}
