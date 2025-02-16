using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class OvertimeController : Controller
{
    private readonly IOvertimeService _service;

    public OvertimeController(IOvertimeService service)
    {
        _service = service;
    }

    [HttpPost("api/overtime/create")]
    public async Task<ActionResult> Create([FromBody] OvertimeRecordDto model)
    {
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("api/overtime/update")]
    public async Task<ActionResult> Update([FromBody] OvertimeRecordDto model)
    {
        var result = await _service.UpdateAsync(model);
        return Ok(result);
    }

    [HttpGet("api/overtime/getById/{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpGet("api/overtime/getAll")]
    public async Task<ActionResult> GetAllToday()
    {
        var result = await _service.GetAllTodayAsync();
        return Ok(result);
    }

    [HttpGet("api/overtime/getByEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    [HttpPost("api/overtime/getByDateRange")]
    public async Task<ActionResult> GetByDateRange([FromBody] DateQuery model)
    {
        var result = await _service.GetByDateRangeAsync(model.StartDate, model.EndDate);
        return Ok(result);
    }

    [HttpPost("api/overtime/getByDateRangeAndEmployeeId")]
    public async Task<ActionResult> GetByEmployeeAndDateRange( [FromBody] EmployeeDateQuery model)
    {
        var result = await _service.GetByEmployeeAndDateRangeAsync(model.EmployeeId, model.StartDate, model.EndDate);
        return Ok(result);
    }

    [HttpPut("api/overtime/approve/{id:int}/{userId:int}")]
    public async Task<ActionResult> ApproveOvertime(int id, int userId)
    {
        var result = await _service.ApproveOvertimeAsync(id, userId);
        return Ok(result);
    }

    [HttpDelete("api/overtime/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }

    public class DateQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class EmployeeDateQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EmployeeId { get; set; }
    }
}