using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class AttendanceService : IAttendanceService
{
    private readonly ProjectPosDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttendanceService> _logger;

    public AttendanceService(
        ProjectPosDbContext context,
        IMapper mapper,
        ILogger<AttendanceService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<AttendanceDto>> CreateAsync(AttendanceDto attendanceDto)
    {
        try
        {
            var attendance = _mapper.Map<Attendance>(attendanceDto);
            var newAttendance = await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
            
            var response = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(x => x.Id == newAttendance.Entity.Id);

            var mappedResult = _mapper.Map<AttendanceDto>(response);
            return ServiceResponse<AttendanceDto>.Success(mappedResult, "Attendance record created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create attendance record");
            return ServiceResponse<AttendanceDto>.Failure($"Failed to create attendance record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<AttendanceDto>> UpdateAsync(AttendanceDto attendanceDto)
    {
        try
        {
            var attendance = _mapper.Map<Attendance>(attendanceDto);
            var existing = _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();

            var response = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(x => x.Id == existing.Entity.Id);

            var mappedResult = _mapper.Map<AttendanceDto>(response);
            return ServiceResponse<AttendanceDto>.Success(mappedResult, "Attendance record updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update attendance record");
            return ServiceResponse<AttendanceDto>.Failure($"Failed to update attendance record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<AttendanceDto>> GetByIdAsync(int id)
    {
        try
        {
            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
                return ServiceResponse<AttendanceDto>.Failure("Attendance record not found");

            var mappedResult = _mapper.Map<AttendanceDto>(attendance);
            return ServiceResponse<AttendanceDto>.Success(mappedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve attendance record");
            return ServiceResponse<AttendanceDto>.Failure($"Failed to retrieve attendance record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<AttendanceDto>>> GetByEmployeeIdAsync(int employeeId)
    {
        try
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<AttendanceDto>>(attendances);
            return ServiceResponse<List<AttendanceDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve attendance records for employee {EmployeeId}", employeeId);
            return ServiceResponse<List<AttendanceDto>>.Failure($"Failed to retrieve attendance records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<AttendanceDto>>> GetAllTodayAsync()
    {
        try
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Date.Date == DateTime.Today.Date)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<AttendanceDto>>(attendances);
            return ServiceResponse<List<AttendanceDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve attendance records for today");
            return ServiceResponse<List<AttendanceDto>>.Failure($"Failed to retrieve attendance records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<AttendanceDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<AttendanceDto>>(attendances);
            return ServiceResponse<List<AttendanceDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve attendance records for date range");
            return ServiceResponse<List<AttendanceDto>>.Failure($"Failed to retrieve attendance records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<AttendanceDto>>> GetByEmployeeAndDateRangeAsync(
        int employeeId, 
        DateTime startDate, 
        DateTime endDate)
    {
        try
        {
            var attendances = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && 
                           a.Date >= startDate && 
                           a.Date <= endDate)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<AttendanceDto>>(attendances);
            return ServiceResponse<List<AttendanceDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve attendance records for employee {EmployeeId} in date range", employeeId);
            return ServiceResponse<List<AttendanceDto>>.Failure($"Failed to retrieve attendance records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
                return ServiceResponse<bool>.Failure("Attendance record not found");

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true, "Attendance record deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attendance record {Id}", id);
            return ServiceResponse<bool>.Failure($"Failed to delete attendance record: {ex.Message}");
        }
    }
}