using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class IncomeStatementController : Controller
{
    private readonly IIncomeStatementService _service;

    public IncomeStatementController(IIncomeStatementService service)
    {
        _service = service;
    }

    [HttpGet("api/incomestatement")]
    public async Task<ActionResult> GenerateIncomeStatement(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await _service.GenerateIncomeStatementAsync(startDate, endDate);
        return Ok(result);
    }
}