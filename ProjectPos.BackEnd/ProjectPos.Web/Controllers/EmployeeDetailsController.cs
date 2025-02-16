using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class EmployeeDetailsController : Controller
{
    private readonly IEmployeeDetailsService _service;

    public EmployeeDetailsController(IEmployeeDetailsService service)
    {
        _service = service;
    }

    [HttpPost("api/employeedetails/create")]
    public async Task<ActionResult> Create([FromBody] EmployeeDetailsDto model)
    {
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("api/employeedetails/update")]
    public async Task<ActionResult> Update([FromBody] EmployeeDetailsDto model)
    {
        var result = await _service.UpdateAsync(model);
        return Ok(result);
    }

    [HttpGet("api/employeedetails/getById/{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/employeedetails/getByEmployeeId/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _service.GetByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    [HttpGet("api/employeedetails/getAll")]
    public async Task<ActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpDelete("api/employeedetails/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return Ok(result);
    }
}