using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMartApi.Models;

namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : Controller
    {
        private readonly IStoreRepository _storeRepository;

        public StoreController(IStoreRepository contactMasterRepo)
        {
            _storeRepository = contactMasterRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetByID(int id)
        {
            return await _storeRepository.GetByID(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetAll()
        {
            return await _storeRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<Store>> Edit([FromBody] Store contactMaster)
        {
            if (contactMaster == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid State");
            }

            return await _storeRepository.Edit(contactMaster);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Store>> DeleteById(int id)
        {
            return await _storeRepository.Delete(id);
        }
    }

}
