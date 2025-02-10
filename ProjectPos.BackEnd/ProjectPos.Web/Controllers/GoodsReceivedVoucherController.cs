using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class GoodsReceivedVoucherController : Controller
    {
        private readonly IGoodsReceivedVoucherService _service;

        public GoodsReceivedVoucherController(IGoodsReceivedVoucherService service)
        {
            _service = service;
        }

        [HttpPost("api/createGoodsReceived")]
        public ActionResult Create([FromBody] GoodsReceivedVoucherDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateGoodsReceived")]
        public async Task<ActionResult> Update([FromBody] GoodsReceivedVoucherDto model)
        {
            var res = await _service.Update(model);
            return Ok(res);
        }

        [HttpPut("api/approveGoodsReceived")]
        public async Task<ActionResult> Approve([FromBody] GoodsReceivedVoucherDto model)
        {
            var res = await _service.Approve(model);
            return Ok(res);
        }

        [HttpGet("api/getGoodsReceivedById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllGoodsReceivedVouchers")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("api/getAllTodayGoodsReceivedVouchers")]
        public async Task<ActionResult> GetAllToday()
        {
            return Ok(await _service.GetAllToday());
        }

        [HttpGet("api/getAllByDateGoodsReceivedVouchers")]
        public async Task<ActionResult> GetAllByDate([FromQuery] DateTime date)
        {
            return Ok(await _service.GetAllByDate(date));
        }

        [HttpGet("api/getAllByDateRangeGoodsReceivedVouchers")]
        public async Task<ActionResult> GetAllByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            return Ok(await _service.GetAllByDateRange(start, end));
        }

        [HttpGet("api/getAllByMonthGoodsReceivedVouchers/{month:int}")]
        public async Task<ActionResult> GetAllByMonth(int month)
        {
            return Ok(await _service.GetAllByMonth(month));
        }

        [HttpGet("api/getAllBySupplierGoodsReceivedVouchers/{supplierId:int}")]
        public async Task<ActionResult> GetAllBySupplier(int supplierId)
        {
            return Ok(await _service.GetAllBySupplier(supplierId));
        }

        [HttpDelete("api/deleteGoodsReceived/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
