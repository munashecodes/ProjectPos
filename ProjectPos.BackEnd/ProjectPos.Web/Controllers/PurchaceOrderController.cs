using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class PurchaceOrderController : Controller
    {
        private readonly IPurchaceOrderService _service;

        public PurchaceOrderController(IPurchaceOrderService service)
        {
            _service = service;
        }

        [HttpPost("api/createPurchaceOrder")]
        public ActionResult Create([FromBody] PurchaceOrderDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updatePurchaceOrder")]
        public ActionResult Update([FromBody] PurchaceOrderDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getPurchaceOrderById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllPurchaceOrders")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deletePurchaceOrder/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }

        [HttpGet("api/getAllTodayPurchaceOrders")]
        public async Task<ActionResult> GetAllToday()
        {
            return Ok(await _service.GetAllToday());
        }

        [HttpGet("api/getAllByDatePurchaceOrders")]
        public async Task<ActionResult> GetAllByDate([FromQuery] DateTime date)
        {
            return Ok(await _service.GetAllByDate(date));
        }

        [HttpGet("api/getAllByDateRangePurchaceOrders")]
        public async Task<ActionResult> GetAllByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            return Ok(await _service.GetAllByDateRange(start, end));
        }

        [HttpGet("api/getAllByMonthPurchaceOrders/{month:int}")]
        public async Task<ActionResult> GetAllByMonth(int month)
        {
            return Ok(await _service.GetAllByMonth(month));
        }

        [HttpGet("api/getAllBySupplierPurchaceOrders/{supplierId:int}")]
        public async Task<ActionResult> GetAllBySupplier(int supplierId)
        {
            return Ok(await _service.GetAllBySupplier(supplierId));
        }
    }
}
