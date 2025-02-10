using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ProductInventorySnapShotController : Controller
    {
        private readonly IProductInventorySnapShotService _service;

        public ProductInventorySnapShotController(IProductInventorySnapShotService service)
        {
            _service = service;
        }

        [HttpPost("api/createInventorySnapShot")]
        public ActionResult Create([FromBody] InventorySnapShotSummaryDto snapShot)
        {
            var result = _service.CreateInventorySnapshots(snapShot);
            return Ok(result);
        }

        [HttpGet("api/getInventorySnapShot/")]
        public ActionResult GetAll([FromQuery] DateTime date)
        {
            var result = _service.GetSnapShotByDate(date);
            return Ok(result);
        }
    }
}
