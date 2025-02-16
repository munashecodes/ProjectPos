using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrialBalanceController : Controller
{
    private readonly ITrialBalanceService _service;
    public TrialBalanceController(ITrialBalanceService service)
    {
        _service = service;
    }

    [HttpPost("get")]
    public async Task<ActionResult<ServiceResponse<List<TrialBalanceAccountsDto>>>> GetByDateRange(DateRangeDto model)
    {
        var res = await _service.GetTrialBalanceByDateRangeAsync(model.Start, model.End);
        return Ok(res);
    }
   
}