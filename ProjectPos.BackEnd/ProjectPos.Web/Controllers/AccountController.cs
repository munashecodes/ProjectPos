using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet("api/getAllAccounts")]
        public async Task<ActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return Ok(res);
        }
    }
}
