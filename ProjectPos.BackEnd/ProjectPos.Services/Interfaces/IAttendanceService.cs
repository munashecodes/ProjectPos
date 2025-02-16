using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IAttendanceService
{
    Task<ServiceResponse<AttendanceDto>> CreateAsync(AttendanceDto attendanceDto);
    Task<ServiceResponse<AttendanceDto>> UpdateAsync(AttendanceDto attendanceDto);
    Task<ServiceResponse<AttendanceDto>> GetByIdAsync(int id);
    Task<ServiceResponse<List<AttendanceDto>>> GetByEmployeeIdAsync(int employeeId);
    
    Task<ServiceResponse<List<AttendanceDto>>> GetAllTodayAsync();
    Task<ServiceResponse<List<AttendanceDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<ServiceResponse<List<AttendanceDto>>> GetByEmployeeAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<ServiceResponse<bool>> DeleteAsync(int id);
}