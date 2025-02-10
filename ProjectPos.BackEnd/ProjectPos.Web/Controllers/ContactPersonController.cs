using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ContactPersonController : Controller
    {
        private readonly IContactPersonService _service;

        public ContactPersonController(IContactPersonService service)
        {
            _service = service;
        }

        [HttpPost("api/createContactPerson")]
        public ActionResult Create([FromBody] ContactPersonDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateContactPerson")]
        public ActionResult Update([FromBody] ContactPersonDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getContactPersonById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllContactPersons")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deleteContactPerson/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
