using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderService _service;

        public SalesOrderController(ISalesOrderService service)
        {
            _service = service;
        }

        [HttpPost("api/createSalesOrder")]
        public ActionResult Create([FromBody] SalesOrderDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateSalesOrder")]
        public ActionResult Update([FromBody] SalesOrderDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getSalesOrderById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getSalesOrderByCustomerId/{id:int}")]
        public ActionResult GetByCustomerId(int id)
        {
            return Ok(_service.GetByCustomerId(id));
        }

        [HttpGet("api/getSalesOrderByName")]
        public ActionResult GetByName([FromQuery] string name)
        {
            return Ok(_service.GetByName(name));
        }

        [HttpGet("api/getAllSalesOrders")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("api/getAllTodaySalesOrders")]
        public async Task<ActionResult> GetAllToday()
        {
            return Ok(await _service.GetAllToday());
        }

        [HttpGet("api/getAllSalesOrderItems")]
        public ActionResult GetSalesOrderItems()
        {
            return Ok(_service.GetSalesOrderItems());
        }

        [HttpGet("api/getAllMonthSalesOrderItems/{month:int}")]
        public ActionResult GetMonthSalesOrderItems(int month)
        {
            return Ok(_service.GetMonthSalesOrderItems(month));
        }

        [HttpGet("api/getAllSalesOrderItem")]
        public ActionResult GetSalesOrderItems([FromQuery]  int id, [FromQuery] DateTime date)
        {
            return Ok(_service.GetSalesOrderItems(date, id));
        }

        [HttpGet("api/getAllOrdersByDate")]
        public ActionResult GetAll([FromQuery] DateTime date)
        {
            return Ok(_service.GetAll(date));
        }

        [HttpGet("api/getAllOrdersByMonth/{month:int}")]
        public ActionResult GetAllMonth(int month)
        {
            return Ok(_service.GetAllMonth(month));
        }

        [HttpGet("api/getAllOrdersByRange")]
        public ActionResult GetAll([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_service.GetAll(startDate, endDate));
        }

        [HttpGet("api/getAllSalesOrdersByDate")]
        public ActionResult GetAllDate([FromQuery] DateTime date)
        {
            return Ok(_service.GetAllDate(date));
        }

        [HttpGet("api/getAllSalesOrdersByRange")]
        public ActionResult GetAllRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_service.GetAllRange(startDate, endDate));
        }

        [HttpDelete("api/deleteSalesOrder/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
