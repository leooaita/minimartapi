using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMartApi.Models;
using MiniMartApi.Interfaces;
using MiniMart.Models;

namespace MiniMartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : Controller
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherController(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetByID(string id)
        {
            return await _voucherRepository.GetByID(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<Voucher>>> GetAll()
        {
            return await _voucherRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<VoucherDiscountPercentPerUnit>> Edit([FromBody] VoucherDiscountPercentPerUnit voucher)
        {
            if (voucher == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid State");
            }

            return await _voucherRepository.Edit((VoucherDiscountPercentPerUnit)voucher);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Voucher>> DeleteById(int id)
        {
            return await _voucherRepository.Delete(id);
        }
    }

}
