using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class DeductionService : IDeductionService
{
    private readonly ProjectPosDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<DeductionService> _logger;

    public DeductionService(
        ProjectPosDbContext context,
        IMapper mapper,
        ILogger<DeductionService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ServiceResponse<EmployeeDeductionDto>> CreateAsync(EmployeeDeductionDto deductionDto)
    {
        try
        {
            var deduction = _mapper.Map<EmployeeDeduction>(deductionDto);
            deduction.IsApproved = false; // New deductions are not approved by default

            var newDeduction = await _context.EmployeeDeductions.AddAsync(deduction);
            await _context.SaveChangesAsync();
            
            var response =  await _context.EmployeeDeductions.
                Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.EmployeeId == newDeduction.Entity.Id);

            var mappedResult = _mapper.Map<EmployeeDeductionDto>(response);
            return ServiceResponse<EmployeeDeductionDto>.Success(mappedResult, "EmployeeDeduction record created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create deduction record");
            return ServiceResponse<EmployeeDeductionDto>.Failure($"Failed to create deduction record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<EmployeeDeductionDto>> UpdateAsync(EmployeeDeductionDto deductionDto)
    {
        try
        {
            var existingRecord = await _context.EmployeeDeductions.FindAsync(deductionDto.Id);
            if (existingRecord == null)
                return ServiceResponse<EmployeeDeductionDto>.Failure("EmployeeDeduction record not found");

            var isApproved = existingRecord.IsApproved;
            _mapper.Map(deductionDto, existingRecord);
            existingRecord.IsApproved = isApproved;

            var existing = _context.EmployeeDeductions.Update(existingRecord);
            await _context.SaveChangesAsync();
            var response =  await _context.EmployeeDeductions.
                Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.EmployeeId == existing.Entity.Id);

            var mappedResult = _mapper.Map<EmployeeDeductionDto>(existingRecord);
            return ServiceResponse<EmployeeDeductionDto>.Success(mappedResult, "EmployeeDeduction record updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update deduction record");
            return ServiceResponse<EmployeeDeductionDto>.Failure($"Failed to update deduction record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<EmployeeDeductionDto>> GetByIdAsync(int id)
    {
        try
        {
            var deduction = await _context.EmployeeDeductions
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deduction == null)
                return ServiceResponse<EmployeeDeductionDto>.Failure("EmployeeDeduction record not found");

            var mappedResult = _mapper.Map<EmployeeDeductionDto>(deduction);
            return ServiceResponse<EmployeeDeductionDto>.Success(mappedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve deduction record");
            return ServiceResponse<EmployeeDeductionDto>.Failure($"Failed to retrieve deduction record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByEmployeeIdAsync(int employeeId)
    {
        try
        {
            var deductions = await _context.EmployeeDeductions
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId)
                .OrderByDescending(d => d.DeductionDate)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<EmployeeDeductionDto>>(deductions);
            return ServiceResponse<List<EmployeeDeductionDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve deduction records for employee {EmployeeId}", employeeId);
            return ServiceResponse<List<EmployeeDeductionDto>>.Failure($"Failed to retrieve deduction records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var deductions = await _context.EmployeeDeductions
                .Include(d => d.Employee)
                .Where(d => d.DeductionDate.Date >= startDate.Date && d.DeductionDate.Date <= endDate.Date)
                .OrderByDescending(d => d.DeductionDate)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<EmployeeDeductionDto>>(deductions);
            return ServiceResponse<List<EmployeeDeductionDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve deduction records for date range");
            return ServiceResponse<List<EmployeeDeductionDto>>.Failure($"Failed to retrieve deduction records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<EmployeeDeductionDto>>> GetByEmployeeAndDateRangeAsync(
        int employeeId,
        DateTime startDate,
        DateTime endDate)
    {
        try
        {
            var deductions = await _context.EmployeeDeductions
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId &&
                           d.DeductionDate >= startDate &&
                           d.DeductionDate <= endDate)
                .OrderByDescending(d => d.DeductionDate)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<EmployeeDeductionDto>>(deductions);
            return ServiceResponse<List<EmployeeDeductionDto>>.Success(mappedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve deduction records for employee {EmployeeId} in date range", employeeId);
            return ServiceResponse<List<EmployeeDeductionDto>>.Failure($"Failed to retrieve deduction records: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<EmployeeDeductionDto>> ApproveDeductionAsync(int id)
    {
        try
        {
            var deduction = await _context.EmployeeDeductions.FindAsync(id);
            if (deduction == null)
                return ServiceResponse<EmployeeDeductionDto>.Failure("EmployeeDeduction record not found");

            deduction.IsApproved = true;
            await _context.SaveChangesAsync();
            
            var approvedDeduction = await _context.EmployeeDeductions
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id);

            var mappedResult = _mapper.Map<EmployeeDeductionDto>(approvedDeduction);
            return ServiceResponse<EmployeeDeductionDto>.Success(mappedResult, "EmployeeDeduction record approved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to approve deduction record");
            return ServiceResponse<EmployeeDeductionDto>.Failure($"Failed to approve deduction record: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var deduction = await _context.EmployeeDeductions.FindAsync(id);
            if (deduction == null)
                return ServiceResponse<bool>.Failure("EmployeeDeduction record not found");

            _context.EmployeeDeductions.Remove(deduction);
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.Success(true, "EmployeeDeduction record deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete deduction record {Id}", id);
            return ServiceResponse<bool>.Failure($"Failed to delete deduction record: {ex.Message}");
        }
    }
}