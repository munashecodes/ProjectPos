using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IOvertimeService
{
    Task<ServiceResponse<OvertimeRecordDto>> CreateAsync(OvertimeRecordDto overtimeDto);
    Task<ServiceResponse<OvertimeRecordDto>> UpdateAsync(OvertimeRecordDto overtimeDto);
    Task<ServiceResponse<OvertimeRecordDto>> GetByIdAsync(int id);
    Task<ServiceResponse<List<OvertimeRecordDto>>> GetByEmployeeIdAsync(int employeeId);
    
    Task<ServiceResponse<List<OvertimeRecordDto>>> GetAllTodayAsync();
    Task<ServiceResponse<List<OvertimeRecordDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ServiceResponse<List<OvertimeRecordDto>>> GetByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<ServiceResponse<OvertimeRecordDto>> ApproveOvertimeAsync(int id, int userId);
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}