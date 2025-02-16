using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class GoodsReceivedVoucherLineController : Controller
{
    private readonly IGoodsReceivedVoucherLineService _service;
    public GoodsReceivedVoucherLineController(IGoodsReceivedVoucherLineService service)
    {
        _service = service;
    }

    [HttpGet("api/getMonthGrvLines/{month:int}")]
    public ActionResult GetMonthGrvLines(int month)
    {
        var response = _service.GetByMonthGrvItems(month);
        return Ok(response);
    }

    [HttpGet("api/getDateGrvLines")]
    public ActionResult GetDateGrvLines([FromQuery] DateTime date)
    {
        var response = _service.GetByDateGrvItems(date);
        return Ok(response);
    }

    [HttpGet("api/getRangeGrvLines")]
    public ActionResult GetRangeGrvLines([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var response = _service.GetGrvItemsByRange(start, end);
        return Ok(response);
    }

    [HttpGet("api/getTodayGrvLines")]
    public ActionResult GetTodayGrvLines()
    {
        var response = _service.GetTodayGrvItems();
        return Ok(response);
    }
}