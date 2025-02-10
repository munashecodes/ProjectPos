using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost("api/createEmployee")]
        public ActionResult AddProfile([FromBody] EmployeeDto employeeModels)
        {
            return Ok(_employeeService.Create(employeeModels));
        }

        [HttpPut("api/updateEmployee")]
        public ActionResult UpdateProfile([FromBody] EmployeeDto employeeModels)
        {
            return Ok(_employeeService.Update(employeeModels));
        }

        [HttpDelete("api/deleteEmployeeById/{userId:int}")]
        public ActionResult Delete(int userId)
        {
            var employee = _employeeService.Delete(userId);
            return Ok(employee);
        }

        [HttpGet("api/getEmployeeById/{userId:int}")]
        public ActionResult GetProfileById(int userId)
        {
            var employee = _employeeService.GetById(userId);
            return Ok(employee);
        }

        [HttpGet("api/getEmployeeByName")]
        public ActionResult GetProfileByName(string name)
        {
            var employee = _employeeService.GetByName(name);
            return Ok(employee);
        }

        [HttpGet("api/getAllEmployees")]
        public ActionResult GetProfiles()
        {
            var employees = _employeeService.GetAll();
            return Ok(employees);
        }
    }
}
