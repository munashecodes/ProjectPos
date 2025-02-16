using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class EmployeeDetailsService : IEmployeeDetailsService
{
    private readonly ProjectPosDbContext _context;
private readonly IMapper _mapper;
private readonly ILogger<EmployeeDetailsService> _logger;

public EmployeeDetailsService(
    ProjectPosDbContext context,
    IMapper mapper,
    ILogger<EmployeeDetailsService> logger)
{
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
}

public async Task<ServiceResponse<EmployeeDetailsDto>> CreateAsync(EmployeeDetailsDto detailsDto)
{
    try
    {
        // Check if employee exists
        var employee = await _context.Employees!.FindAsync(detailsDto.EmployeeId);
        if (employee == null)
            return ServiceResponse<EmployeeDetailsDto>.Failure("Employee not found");

        // Check if details already exist for this employee
        var existingDetails = await _context.EmployeeDetails
            .FirstOrDefaultAsync(d => d.EmployeeId == detailsDto.EmployeeId);
        if (existingDetails != null)
            return ServiceResponse<EmployeeDetailsDto>.Failure("Employee details already exist");

        var details = _mapper.Map<EmployeeDetails>(detailsDto);
        await _context.EmployeeDetails.AddAsync(details);
        await _context.SaveChangesAsync();

        var mappedResult = _mapper.Map<EmployeeDetailsDto>(details);
        return ServiceResponse<EmployeeDetailsDto>.Success(mappedResult, "Employee details created successfully");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to create employee details");
        return ServiceResponse<EmployeeDetailsDto>.Failure($"Failed to create employee details: {ex.Message}");
    }
}

public async Task<ServiceResponse<EmployeeDetailsDto>> UpdateAsync(EmployeeDetailsDto detailsDto)
{
    try
    {
        var existingDetails = await _context.EmployeeDetails
            .Include(d => d.Employee)
            .FirstOrDefaultAsync(d => d.Id == detailsDto.Id);

        if (existingDetails == null)
            return ServiceResponse<EmployeeDetailsDto>.Failure("Employee details not found");

        _mapper.Map(detailsDto, existingDetails);
        _context.EmployeeDetails.Update(existingDetails);
        await _context.SaveChangesAsync();

        var mappedResult = _mapper.Map<EmployeeDetailsDto>(existingDetails);
        return ServiceResponse<EmployeeDetailsDto>.Success(mappedResult, "Employee details updated successfully");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to update employee details");
        return ServiceResponse<EmployeeDetailsDto>.Failure($"Failed to update employee details: {ex.Message}");
    }
}

public async Task<ServiceResponse<EmployeeDetailsDto>> GetByIdAsync(int id)
{
    try
    {
        var details = await _context.EmployeeDetails
            .Include(d => d.Employee)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (details == null)
            return ServiceResponse<EmployeeDetailsDto>.Failure("Employee details not found");

        var mappedResult = _mapper.Map<EmployeeDetailsDto>(details);
        return ServiceResponse<EmployeeDetailsDto>.Success(mappedResult);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to retrieve employee details");
        return ServiceResponse<EmployeeDetailsDto>.Failure($"Failed to retrieve employee details: {ex.Message}");
    }
}

public async Task<ServiceResponse<EmployeeDetailsDto>> GetByEmployeeIdAsync(int employeeId)
{
    try
    {
        var details = await _context.EmployeeDetails
            .Include(d => d.Employee)
            .FirstOrDefaultAsync(d => d.EmployeeId == employeeId);

        if (details == null)
            return ServiceResponse<EmployeeDetailsDto>.Failure("Employee details not found");

        var mappedResult = _mapper.Map<EmployeeDetailsDto>(details);
        return ServiceResponse<EmployeeDetailsDto>.Success(mappedResult);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to retrieve employee details");
        return ServiceResponse<EmployeeDetailsDto>.Failure($"Failed to retrieve employee details: {ex.Message}");
    }
}

public async Task<ServiceResponse<List<EmployeeDetailsDto>>> GetAllAsync()
{
    try
    {
        var details = await _context.EmployeeDetails
            .Include(d => d.Employee)
            .OrderBy(d => d.Employee!.Name)
            .ToListAsync();

        var mappedResults = _mapper.Map<List<EmployeeDetailsDto>>(details);
        return ServiceResponse<List<EmployeeDetailsDto>>.Success(mappedResults);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to retrieve all employee details");
        return ServiceResponse<List<EmployeeDetailsDto>>.Failure($"Failed to retrieve employee details: {ex.Message}");
    }
}

public async Task<ServiceResponse<bool>> DeleteAsync(int id)
{
    try
    {
        var details = await _context.EmployeeDetails.FindAsync(id);
        if (details == null)
            return ServiceResponse<bool>.Failure("Employee details not found");

        _context.EmployeeDetails.Remove(details);
        await _context.SaveChangesAsync();

        return ServiceResponse<bool>.Success(true, "Employee details deleted successfully");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to delete employee details {Id}", id);
        return ServiceResponse<bool>.Failure($"Failed to delete employee details: {ex.Message}");
    }
}
}