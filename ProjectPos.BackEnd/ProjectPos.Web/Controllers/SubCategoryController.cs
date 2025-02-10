using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _service;

        public SubCategoryController(ISubCategoryService service)
        {
            _service = service;
        }

        [HttpPost("api/createSubCategory")]
        public ActionResult Create([FromBody] SubCategoryDto model)
        {
            return Ok(_service.Create(model));
        }

        [HttpPut("api/updateSubCategory")]
        public ActionResult Update([FromBody] SubCategoryDto model)
        {
            return Ok(_service.Update(model));
        }

        [HttpGet("api/getSubCategoryById/{id:int}")]
        public ActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpGet("api/getAllSubCategorys")]
        public ActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpDelete("api/deleteSubCategory/{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_service.Delete(id));
        }
    }
}
