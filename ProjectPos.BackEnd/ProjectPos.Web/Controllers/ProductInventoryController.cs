using Microsoft.AspNetCore.Mvc;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ProductInventoryController : Controller
    {
        private readonly IProductInventoryService _service;

        public ProductInventoryController(IProductInventoryService service)
        {
            _service = service;
        }

        [HttpPost("api/createProductInventory")]
        public ActionResult Create([FromBody] ProductInventoryDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateProductInventory")]
        public ActionResult Update([FromBody] ProductInventoryDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpPut("api/updateRangeProductInventory")]
        public ActionResult UpdateRange([FromBody] List<ProductInventoryDto> model)
        {
            return Ok(_service.UpdateRange(model));
        }

        [HttpGet("api/getProductInventoryById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllProductInventories")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("api/getAllPricedProductInventories")]
        public ActionResult GetAlToSelll()
        {
            return Ok(_service.GetAllToSell());
        }

        [HttpDelete("api/deleteProductInventory/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }

        [HttpGet("api/getProductInventoryByCode")]
        public ActionResult GetByBarCode([FromQuery] string barCode)
        {
            return Ok(_service.GetByName(barCode));
        }

        [HttpGet("api/getProductInventoryPlu")]
        public ActionResult GetByPlu([FromQuery] string plu)
        {
            return Ok(_service.GetByPlu(plu));
        }

        [HttpPost("api/generateInventory")]
        public async Task<ActionResult> GenerateInventory()
        {
            var response = await _service.GenerateInventory();
            return Ok(response);
        }

        [HttpPost("api/generateStockTakeReport")]
        public async Task<ActionResult> GenerateStockTakeReport([FromQuery] Category department)
        {
            var response = await _service.GenerateStockTakeReport(department);
            return Ok(response);
        }

        [HttpPost("api/generateStockValueReport")]
        public async Task<ActionResult> GenerateStockValueReport([FromQuery] Category department)
        {
            var response = await _service.GenerateStockValueReport(department);
            return Ok(response);
        }

        [HttpPost("api/generateStockTakeLog")]
        public async Task<ActionResult> GenerateStockTakeLog([FromBody] StockTakeLogDto log)
        {
            var response = await _service.CreateStockTakeLog(log);
            return Ok(response);
        }

        [HttpPost("api/closeStockTakeLog")]
        public async Task<ActionResult> CloseStockTakeLog([FromBody] StockTakeLogDto log)
        {
            var response = await _service.CloseStockTakeLog(log);
            return Ok(response);
        }

        [HttpPost("api/checkStockTakeLog")]
        public async Task<ActionResult> CheckStockTakeLog([FromQuery] Category department)
        {
            var response = await _service.CheckStockTakeLog(department);
            return Ok(response);
        }
    }
}
