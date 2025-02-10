using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.AppServices;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationService _jwtService;
        private readonly IUserService _userService;

        [ActivatorUtilitiesConstructor]
        public UserController(
            IAuthenticationService jwtService,
            IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("api/login")]
        public ActionResult Login([FromBody] UserDto user)
        {
            var _user = _userService.Login(user);
            return Ok(_user);
        }

        [HttpPost("api/logout")]
        public ActionResult Logout([FromBody] UserDto user)
        {
            var _user = _userService.LogOut(user);
            return Ok(_user);
        }

        [HttpDelete("api/deleteUser/{userId:int}")]
        public ActionResult Delete(int userId)
        {
            var employee = _userService.Delete(userId);
            return Ok(employee);
        }

        [HttpPost("api/signin")]
        public ActionResult CreateAccount([FromBody] UserSignInDto user)
        {
            return Ok(_userService.Create(user));
        }

        [HttpPut("api/update")]
        public ActionResult UpdateAccount([FromBody] UserDto user)
        {
            return Ok(_userService.Update(user));
        }

        [HttpPost("api/verifyToken")]
        public ActionResult VerifyToken([FromBody] UserDto user)
        {
            return Ok(_jwtService.ValidateJwtToken(user));
        }

        [HttpGet("api/getAll")]
        public ActionResult GetAllUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("getById{id:int}")]
        public ActionResult GetUserById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPost("api/verifySupervisor")]
        public ActionResult GetAllUsers([FromQuery] string code)
        {
            var users = _userService.VerifySupervisor(code);
            return Ok(users);
        }


        //[HttpDelete("api/deleteUser/{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    return Ok(_userService.Delete(id));
        //}
    }
}
