using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ProofOfPaymentController : Controller
    {
        private readonly IProofOfPaymentService _service;
        private readonly IMapper _mapper;

        public ProofOfPaymentController(IProofOfPaymentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("api/createProofOfPayment")]
        public ActionResult Create([FromBody] ProofOfPaymentDto proofOfPayment)
        {
            var response = _service.Create(proofOfPayment);
            return Ok(response);
        }

        [HttpDelete("api/deleteProofOfPayment/{id:int}")]
        public ActionResult Delete(int id)
        {
            var response = _service.Delete(id);
            return Ok(response);
        }

        [HttpGet("api/getProofOfPayment/{id:int}")]
        public ActionResult Get(int id)
        {
            var response = _service.Get(id);
            return Ok(response);
        }

        [HttpGet("api/getAllProofOfPayments")]
        public ActionResult GetAll()
        {
            var response = _service.GetAll();
            return Ok(response);
        }
        [HttpGet("api/getAllProofOfPayment/{id:int}")]
        public ActionResult GetAllByCustomerId(int id)
        {
            var response = _service.GetAllByCustomerId(id);
            return Ok(response);
        }

        [HttpPut("api/updateProofOfPayment")]
        public ActionResult Update([FromBody] ProofOfPaymentDto proofOfPayment)
        {
            var response = _service.Update(proofOfPayment);
            return Ok(response);
        }

        [HttpGet("api/getAllProofOfPaymentsByDate")]
        public ActionResult GetAll([FromQuery] DateTime date)
        {
            return Ok(_service.GetAll(date));
        }

        [HttpGet("api/getAllProofOfPaymentsByMonth/{month:int}")]
        public ActionResult GetAllMonth(int month)
        {
            return Ok(_service.GetAllMonth(month));
        }

        [HttpGet("api/getAllProofOfPaymentsByRange")]
        public ActionResult GetAll([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_service.GetAll(startDate, endDate));
        }

        [HttpGet("api/getDateReportByCustomer")]
        public ActionResult GetDateReportByCustomer([FromQuery] DateTime date)
        {
            return Ok(_service.GetDateReportByCustomer(date));
        }

        [HttpGet("api/getRangeReportByCustomer")]
        public ActionResult GetRangeReportByCustomer([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok(_service.GetRangeReportByCustomer(startDate, endDate));
        }

        [HttpGet("api/getMonthReportByCustomer/{month:int}")]
        public ActionResult GetMonthReportByCustomer(int month)
        {
            return Ok(_service.GetMonthReportByCustomer(month));
        }
    }
}
