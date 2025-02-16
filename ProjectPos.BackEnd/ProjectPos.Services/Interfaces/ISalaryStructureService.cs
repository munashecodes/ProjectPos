using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface ISalaryStructureService
{
    Task<ServiceResponse<SalaryStructureDto>> CreateSalaryStructureAsync(SalaryStructureDto salaryStructureDto);
    Task<ServiceResponse<SalaryStructureDto>> UpdateSalaryStructureAsync(SalaryStructureDto salaryStructureDto);
    Task<ServiceResponse<SalaryStructureDto>> GetSalaryStructureByIdAsync(int id);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetAllSalaryStructuresAsync();
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByEmployeeIdAsync(int employeeId);
    /*
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByMonthAsync(int month);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByYearAsync(int year);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByMonthAndYearAsync(int month, int year);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByEmployeeIdAndMonthAsync(int employeeId, int month);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByEmployeeIdAndYearAsync(int employeeId, int year);
    Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByEmployeeIdAndMonthAndYearAsync(int employeeId, int month, int year);
    */
    Task<ServiceResponse<SalaryStructureDto>> DeleteSalaryStructureAsync(int userId, int id);
}