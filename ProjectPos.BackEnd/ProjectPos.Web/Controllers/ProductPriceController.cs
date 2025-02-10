using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ProductPriceController : Controller
    {
        private readonly IProductPriceService _service;

        public ProductPriceController(IProductPriceService service)
        {
            _service = service;
        }

        [HttpPost("api/createProductPrice")]
        public ActionResult Create([FromBody] ProductPriceDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateProductPrice")]
        public ActionResult Update([FromBody] ProductPriceDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getProductPriceById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllProductPrices")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deleteProductPrice/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
