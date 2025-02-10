using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class StockMovementController : Controller
    {
        private readonly IStockMovementService _service;

        public StockMovementController(IStockMovementService service)
        {
            _service = service;
        }

        //add apis corresponding to IStockMovementService methods
        [HttpPost("api/addStockMovements")]
        public async Task<ActionResult> CreateRange([FromBody] StockMovementLogDto movements) 
        {
            var res = await _service.AddAsync(movements);
            return Ok(res);
        }

        //add apis corresponding to IStockMovementService methods
        [HttpPost("api/addStockMovement")]
        public async Task<ActionResult> Create([FromBody] StockMovementLogDto movements)
        {
            var res = await _service.AddAsync(movements);
            return Ok(res);
        }

        //approve stock movement
        [HttpPut("api/approveStockMovement/{id:int}/{userId:int}")]
        public async Task<ActionResult> Approve(int id, int userId)
        {
            var res = await _service.Approve(id, userId);
            return Ok(res);
        }

        //update stock movement
        [HttpPut("api/updateStockMovement")]
        public async Task<ActionResult> Update([FromBody] StockMovementLogDto movements)
        {
            var res = await _service.UpdateAsync(movements);
            return Ok(res);
        }

        //delete stock movement
        [HttpDelete("api/deleteStockMovement/{id:int}/{userId:int}")]
        public async Task<ActionResult> Delete(int id, int userId)
        {
            var res = await _service.DeleteAsync(id, userId);
            return Ok(res);
        }

        //get all stock movement
        [HttpGet("api/getAllStockMovements")]
        public async Task<ActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return Ok(res);
        }

        //get stock movement by today
        [HttpGet("api/getAllStockMovementsToday")]
        public async Task<ActionResult> GetAllToday()
        {
            var res = await _service.GetAllTodayAsync();
            return Ok(res);
        }

        //get stock movement by date
        [HttpGet("api/getAllStockMovementsByDate")]
        public async Task<ActionResult> GetAllByDate([FromQuery]DateTime date)
        {
            var res = await _service.GetAllByDateAsync(date);
            return Ok(res);
        }

        //get stock movement by date range
        [HttpGet("api/getAllStockMovementsByDateRange")]
        public async Task<ActionResult> GetAllByDateRange([FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        {
            var res = await _service.GetAllByDateRangeAsync(startDate, endDate);
            return Ok(res);
        }

        //get stock movement by month
        [HttpGet("api/getAllStockMovementsByMonth/{month:int}")]
        public async Task<ActionResult> GetAllByMonth(int month)
        {
            var res = await _service.GetAllByMonthAsync(month);
            return Ok(res);
        }
    }
}
