using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IDeductionService
{
    Task<ServiceResponse<EmployeeDeductionDto>> CreateAsync(EmployeeDeductionDto deductionDto);
    Task<ServiceResponse<EmployeeDeductionDto>> UpdateAsync(EmployeeDeductionDto deductionDto);
    Task<ServiceResponse<EmployeeDeductionDto>> GetByIdAsync(int id);
    Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByEmployeeIdAsync(int employeeId);
    Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<ServiceResponse<EmployeeDeductionDto>> ApproveDeductionAsync(int id);
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}