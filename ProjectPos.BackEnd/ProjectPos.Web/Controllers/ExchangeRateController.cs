using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ExchangeRateController : Controller
    {
        private readonly IExchangeRateService _service;
        public ExchangeRateController(IExchangeRateService service)
        {
            _service = service;
        }

        [HttpPost("api/createExchangeRate")]
        public ActionResult Create([FromBody] ExchangeRateDto exchangeRate)
        {
            var res = _service.Create(exchangeRate);
            return Ok(res);
        }


        [HttpPut("api/updateExchangeRate")]
        public ActionResult Update([FromBody] ExchangeRateDto exchangeRate)
        {
            var res = _service.Update(exchangeRate);
            return Ok(res);
        }

        [HttpDelete("api/deleteExchangeRate/{id:int}")]
        public ActionResult Delete(int id)
        {
            var res = _service.Delete(id);
            return Ok(res);
        }


        [HttpGet("api/getExchangeRate/{id:int}")]
        public ActionResult Get(int id)
        {
            var res = _service.Get(id);
            return Ok(res);
        }

        [HttpGet("api/getAllExchangeRates")]
        public ActionResult GetAll()
        {
            var res = _service.GetAll();
            return Ok(res);
        }

        [HttpGet("api/getAllRatesByDate")]
        public ActionResult GetByDate([FromQuery] DateTime date)
        {
            var res = _service.GetByDate(date);
            return Ok(res);
        }
    }
}
