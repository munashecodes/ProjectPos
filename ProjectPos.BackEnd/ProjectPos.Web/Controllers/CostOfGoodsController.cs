using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CostOfGoodsController
{
    private readonly ICostOfGoodsReport _costOfGoodsReport;
    public CostOfGoodsController(ICostOfGoodsReport costOfGoodsReport)
    {
        _costOfGoodsReport = costOfGoodsReport;
    }
    
    [HttpGet("cost-of-goods")]
    public async Task<ActionResult<ServiceResponse<CostOfGoodsReportDto>>> GetCostOfGoodsReport()
    {
        var response = await _costOfGoodsReport.GetCostOfGoodsReport();
        return response;
    }
    
    [HttpGet("cogs")]
    public async Task<ActionResult<ServiceResponse<CogsReportDto>>> GetCogsReport(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromQuery] string? timeRange)
    {
        // Handle different filter types (month, year, etc.)
        var (calculatedStart, calculatedEnd) = CalculateDateRange(startDate, endDate, timeRange);
        return await _costOfGoodsReport.GetCogsReport(calculatedStart, calculatedEnd);
    }
    
    private (DateOnly? start, DateOnly? end) CalculateDateRange(DateOnly start, DateOnly? end, string? range)
    {
        // Implement logic for different filter types
        // Example for "month" filter:
        if (range == "current-month")
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            return (new DateOnly(now.Year, now.Month, 1), now);
        }
        return (start, end);
    }
}