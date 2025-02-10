using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class CashUpController : Controller
    {
        private readonly ICashUpService _service;

        public CashUpController(ICashUpService service)
        {
            _service = service;
        }

        [HttpPost("api/createCashUp")]
        public ActionResult Create([FromBody] List<CashUpDto> model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateCashUp")]
        public ActionResult Update([FromBody] CashUpDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getCashUpById/{id:int}")]
        public ActionResult GetById(int id, [FromQuery] DateTime date)
        {
            return Ok(_service.GetByUserId(id, date));
        }

        [HttpGet("api/getReconById/{id:int}")]
        public ActionResult GetReconById(int id, [FromQuery] DateTime date)
        {
            return Ok(_service.GetReconByUserId(id, date));
        }

        [HttpGet("api/getCashUpReconById/{id:int}")]
        public async Task<ActionResult> GetCashUpReconById(int id, [FromQuery] DateTime date)
        {
            var res = await _service.GetUserCashUpByDate(date, id);
            return Ok();
        }

        [HttpGet("api/getAllCashUpRecons")]
        public async Task<ActionResult> GetAllCashUpsByDate([FromQuery] DateTime date)
        {
            var res = await _service.GetAllCashUpsByDate(date);
            return Ok(res);
        }

        [HttpGet("api/getAllCashUps")]
        public ActionResult GetAll([FromQuery] DateTime date)
        {
            return Ok(_service.GetAll(date));
        }

        [HttpGet("api/getAllRecons")]
        public ActionResult GetAllRecons([FromQuery] DateTime date)
        {
            return Ok(_service.GetAllRecon(date));
        }

        [HttpDelete("api/deleteCashUp/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
