using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers;

public class SalaryStructureController : Controller
{
    private readonly ISalaryStructureService _service;

    public SalaryStructureController(ISalaryStructureService service)
    {
        _service = service;
    }

    [HttpPost("api/createSalaryStructure")]
    public async Task<ActionResult> Create([FromBody] SalaryStructureDto model)
    {
        var result = await _service.CreateSalaryStructureAsync(model);
        return Ok(result);
    }

    [HttpPut("api/updateSalaryStructure")]
    public async Task<ActionResult> Update([FromBody] SalaryStructureDto model)
    {
        var result = await _service.UpdateSalaryStructureAsync(model);
        return Ok(result);
    }

    [HttpGet("api/getSalaryStructure/{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _service.GetSalaryStructureByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/getAllSalaryStructures")]
    public async Task<ActionResult> GetAll()
    {
        var result = await _service.GetAllSalaryStructuresAsync();
        return Ok(result);
    }

    [HttpGet("api/getSalaryStructuresByEmployee/{employeeId:int}")]
    public async Task<ActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _service.GetSalaryStructuresByEmployeeIdAsync(employeeId);
        return Ok(result);
    }

    [HttpDelete("api/deleteSalaryStructure/{userId:int}/{id:int}")]
    public async Task<ActionResult> Delete(int userId, int id)
    {
        var result = await _service.DeleteSalaryStructureAsync(userId, id);
        return Ok(result);
    }
}