using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class PurchaceInvoiceController : Controller
    {
        private readonly IPurchaceInvoiceService _service;

        public PurchaceInvoiceController(IPurchaceInvoiceService service)
        {
            _service = service;
        }

        [HttpPost("api/createPurchaceInvoice")]
        public ActionResult Create([FromBody] PurchaceInvoiceDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updatePurchaceInvoice")]
        public ActionResult Update([FromBody] PurchaceInvoiceDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getPurchaceInvoiceById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllPurchaceInvoices")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deletePurchaceInvoice/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
