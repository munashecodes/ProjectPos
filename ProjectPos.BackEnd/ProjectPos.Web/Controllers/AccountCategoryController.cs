using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class AccountCategoryController : Controller
    {
        private readonly IAccountCategoryService _service;
        public AccountCategoryController(IAccountCategoryService service)
        {
            _service = service;
        }

        [HttpGet("api/getAllAccountCategories")]
        public async Task<ActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return Ok(res);
        }
    }
}
