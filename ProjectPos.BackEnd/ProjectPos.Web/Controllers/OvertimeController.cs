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

    [HttpGet("api/overtime/getByDateRange")]
    public async Task<ActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByDateRangeAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("api/overtime/getByDateRangeAndEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeAndDateRange(
        int employeeId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByEmployeeAndDateRangeAsync(employeeId, startDate, endDate);
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
}