using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IEmployeeDetailsService
{
    Task<ServiceResponse<EmployeeDetailsDto>> CreateAsync(EmployeeDetailsDto detailsDto);
    Task<ServiceResponse<EmployeeDetailsDto>> UpdateAsync(EmployeeDetailsDto detailsDto);
    Task<ServiceResponse<EmployeeDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResponse<EmployeeDetailsDto>> GetByEmployeeIdAsync(int employeeId);
    Task<ServiceResponse<List<EmployeeDetailsDto>>> GetAllAsync();
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}