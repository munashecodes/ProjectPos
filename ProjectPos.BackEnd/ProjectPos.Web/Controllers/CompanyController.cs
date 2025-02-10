using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpPost("api/createCompany")]
        public ActionResult Create([FromBody] CompanyDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateCompany")]
        public ActionResult Update([FromBody] CompanyDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getCompanyById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllCompanies")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deleteCompany/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
