using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class OvertimeService : IOvertimeService
{
    private readonly ProjectPosDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<OvertimeService> _logger;

    public OvertimeService(
        ProjectPosDbContext context,
        IMapper mapper,
        ILogger<OvertimeService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<OvertimeRecordDto>> CreateAsync(OvertimeRecordDto overtimeDto)
    {
        try
        {
            var overtime = _mapper.Map<OvertimeRecord>(overtimeDto);
            overtime.IsApproved = false; // New records are not approved by default
            
            await _context.OvertimeRecords.AddAsync(overtime);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<OvertimeRecordDto>(overtime);
            return ServiceResponse<OvertimeRecordDto>.Success(mappedResult, "Overtime record created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create overtime record");
            return ServiceResponse<OvertimeRecordDto>.Failure($"Failed to create overtime record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<OvertimeRecordDto>> UpdateAsync(OvertimeRecordDto overtimeDto)
    {
        try
        {
            var existingRecord = await _context.OvertimeRecords.FindAsync(overtimeDto.Id);
            if (existingRecord == null)
                return ServiceResponse<OvertimeRecordDto>.Failure("Overtime record not found");

            // Preserve the approval status unless explicitly approved through ApproveOvertimeAsync
            var isApproved = existingRecord.IsApproved;
            _mapper.Map(overtimeDto, existingRecord);
            existingRecord.IsApproved = isApproved;

            _context.OvertimeRecords.Update(existingRecord);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<OvertimeRecordDto>(existingRecord);
            return ServiceResponse<OvertimeRecordDto>.Success(mappedResult, "Overtime record updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update overtime record");
            return ServiceResponse<OvertimeRecordDto>.Failure($"Failed to update overtime record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<OvertimeRecordDto>> GetByIdAsync(int id)
    {
        try
        {
            var overtime = await _context.OvertimeRecords
                .Include(o => o.Employee)
                    .ThenInclude(e => e.SalaryStructure)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (overtime == null)
                return ServiceResponse<OvertimeRecordDto>.Failure("Overtime record not found");

            var mappedResult = _mapper.Map<OvertimeRecordDto>(overtime);
            return ServiceResponse<OvertimeRecordDto>.Success(mappedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overtime record");
            return ServiceResponse<OvertimeRecordDto>.Failure($"Failed to retrieve overtime record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<OvertimeRecordDto>>> GetByEmployeeIdAsync(int employeeId)
    {
        try
        {
            var records = await _context.OvertimeRecords
                .Include(o => o.Employee)
                    .ThenInclude(e => e.SalaryStructure)
                .Where(o => o.EmployeeId == employeeId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<OvertimeRecordDto>>(records);
            return ServiceResponse<List<OvertimeRecordDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overtime records for employee {EmployeeId}", employeeId);
            return ServiceResponse<List<OvertimeRecordDto>>.Failure($"Failed to retrieve overtime records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<OvertimeRecordDto>>> GetAllTodayAsync()
    {
        try
        {
            var records = await _context.OvertimeRecords
                .Include(o => o.Employee)
                .ThenInclude(e => e.SalaryStructure)
                .Where(o => o.Date.Date == DateTime.Today.Date)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<OvertimeRecordDto>>(records);
            return ServiceResponse<List<OvertimeRecordDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overtime records for date range");
            return ServiceResponse<List<OvertimeRecordDto>>.Failure($"Failed to retrieve overtime records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<OvertimeRecordDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var records = await _context.OvertimeRecords
                .Include(o => o.Employee)
                    .ThenInclude(e => e.SalaryStructure)
                .Where(o => o.Date >= startDate && o.Date <= endDate)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<OvertimeRecordDto>>(records);
            return ServiceResponse<List<OvertimeRecordDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overtime records for date range");
            return ServiceResponse<List<OvertimeRecordDto>>.Failure($"Failed to retrieve overtime records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<OvertimeRecordDto>>> GetByEmployeeAndDateRangeAsync(
        int employeeId,
        DateTime startDate,
        DateTime endDate)
    {
        try
        {
            var records = await _context.OvertimeRecords
                .Include(o => o.Employee)
                    .ThenInclude(e => e.SalaryStructure)
                .Where(o => o.EmployeeId == employeeId &&
                           o.Date >= startDate &&
                           o.Date <= endDate)
                .OrderByDescending(o => o.Date)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<OvertimeRecordDto>>(records);
            return ServiceResponse<List<OvertimeRecordDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overtime records for employee {EmployeeId} in date range", employeeId);
            return ServiceResponse<List<OvertimeRecordDto>>.Failure($"Failed to retrieve overtime records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<OvertimeRecordDto>> ApproveOvertimeAsync(int id, int userId)
    {
        try
        {
            var overtime = await _context.OvertimeRecords.FindAsync(id);
            if (overtime == null)
                return ServiceResponse<OvertimeRecordDto>.Failure("Overtime record not found");

            overtime.IsApproved = true;
            overtime.ApprovedById = userId;
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<OvertimeRecordDto>(overtime);
            return ServiceResponse<OvertimeRecordDto>.Success(mappedResult, "Overtime record approved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to approve overtime record");
            return ServiceResponse<OvertimeRecordDto>.Failure($"Failed to approve overtime record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var overtime = await _context.OvertimeRecords.FindAsync(id);
            if (overtime == null)
                return ServiceResponse<bool>.Failure("Overtime record not found");

            _context.OvertimeRecords.Remove(overtime);
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true, "Overtime record deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete overtime record {Id}", id);
            return ServiceResponse<bool>.Failure($"Failed to delete overtime record: {ex.Message}");
        }
    }
}