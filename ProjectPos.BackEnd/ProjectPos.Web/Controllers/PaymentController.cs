using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("api/createPayment")]
        public ActionResult Create([FromBody] PaymentDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updatePayment")]
        public ActionResult Update([FromBody] PaymentDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getPaymentById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getPaymentByOrderId/{id:int}")]
        public ActionResult GetByOrder(int id)
        {
            return Ok(_service.GetByOrder(id));
        }

        [HttpGet("api/getAllPayments")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deletePayment/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
