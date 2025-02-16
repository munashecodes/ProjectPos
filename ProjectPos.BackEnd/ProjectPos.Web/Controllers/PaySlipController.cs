using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class PaySlipController : Controller
{
    private readonly IPaySlipService _service;

    public PaySlipController(IPaySlipService service)
    {
        _service = service;
    }

    [HttpPost("api/generatePayRoll/{userId:int}")]
    public async Task<ActionResult> GeneratePayRoll(int userId)
    {
        var result = await _service.GeneratePayRollAsync(userId);
        return Ok(result);
    }

    [HttpPut("api/approvePayRoll/{month:int}/{year:int}/{userId:int}")]
    public async Task<ActionResult> ApprovePayRoll(int month, int year, int userId)
    {
        var result = await _service.ApprovePayRollAsync(month, year, userId);
        return Ok(result);
    }

    [HttpPut("api/editPaySlip")]
    public async Task<ActionResult> EditPaySlip([FromBody] PaySlipDto model)
    {
        var result = await _service.EditPaySlipAsync(model);
        return Ok(result);
    }

    [HttpGet("api/getPayRoll/{month:int}/{year:int}")]
    public async Task<ActionResult> GetPayRoll(int month, int year)
    {
        var result = await _service.GetPayRollAsync(month, year);
        return Ok(result);
    }

    [HttpGet("api/getPaySlip/{month:int}/{year:int}/{employeeId:int}")]
    public async Task<ActionResult> GetPaySlip(int month, int year, int employeeId)
    {
        var result = await _service.GetPaySlipAsync(month, year, employeeId);
        return Ok(result);
    }

    [HttpGet("api/getAllPayRollCycles")]
    public async Task<ActionResult> GetAllPayRollCycles()
    {
        var result = await _service.GetAllPayRollCycles();
        return Ok(result);
    }

    [HttpGet("api/getAllPayRollCyclesByYear/{year:int}")]
    public async Task<ActionResult> GetAllPayRollCyclesByYear(int year)
    {
        var result = await _service.GetAllPayRollCyclesByYear(year);
        return Ok(result);
    }
}