using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IPaySlipService
{
    Task<ServiceResponse<PayRollCycleDto>> GeneratePayRollAsync(int userId);
    Task<ServiceResponse<PayRollCycleDto>> ApprovePayRollAsync(int month, int year, int userId);
    Task<ServiceResponse<PayRollCycleDto>> GetPayRollAsync(int month, int year);
    Task<ServiceResponse<PaySlipDto>> GetPaySlipAsync(int month, int year, int employeeId);
    Task<ServiceResponse<PaySlipDto>> EditPaySlipAsync(PaySlipDto paySlipDto);
    Task<ServiceResponse<List<PayRollCycleDto>>> GetAllPayRollCycles();
    Task<ServiceResponse<List<PayRollCycleDto>>> GetAllPayRollCyclesByYear(int year);

}