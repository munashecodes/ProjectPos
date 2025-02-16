using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class AttendanceController : Controller
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service)
    {
        _service = service;
    }

    [HttpPost("api/attendance/create")]
    public async Task<ActionResult> Create([FromBody] AttendanceDto model)
    {
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("api/attendance/update")]
    public async Task<ActionResult> Update([FromBody] AttendanceDto model)
    {
        var result = await _service.UpdateAsync(model);
        return Ok(result);
    }

    [HttpGet("api/attendance/getById/{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpGet("api/attendance/getAll")]
    public async Task<ActionResult> GetAllToday()
    {
        var result = await _service.GetAllTodayAsync();
        return Ok(result);
    }

    [HttpGet("api/attendance/getByEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    [HttpGet("api/attendance/getByDateRange")]
    public async Task<ActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByDateRangeAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("api/attendance/getByDateRangeAndEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeAndDateRange(
        int employeeId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByEmployeeAndDateRangeAsync(employeeId, startDate, endDate);
        return Ok(result);
    }

    [HttpDelete("api/attendance/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}