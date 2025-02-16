using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class DeductionController : Controller
{
    private readonly IDeductionService _service;

    public DeductionController(IDeductionService service)
    {
        _service = service;
    }

    [HttpPost("api/deduction/create")]
    public async Task<ActionResult> Create([FromBody] EmployeeDeductionDto model)
    {
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("api/deduction/update")]
    public async Task<ActionResult> Update([FromBody] EmployeeDeductionDto model)
    {
        var result = await _service.UpdateAsync(model);
        return Ok(result);
    }

    [HttpGet("api/deduction/getById/{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/deduction/getByEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    [HttpGet("api/deduction/getByDateRange")]
    public async Task<ActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByDateRangeAsync(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("api/deduction/getByEmployeeAndDateRange/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeAndDateRange(
        int employeeId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _service.GetByEmployeeAndDateRangeAsync(employeeId, startDate, endDate);
        return Ok(result);
    }

    [HttpPut("api/deduction/approve/{id:int}")]
    public async Task<ActionResult> ApproveDeduction(int id)
    {
        var result = await _service.ApproveDeductionAsync(id);
        return Ok(result);
    }

    [HttpDelete("api/deduction/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}