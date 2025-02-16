using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class SalaryStructureService : ISalaryStructureService
{
    private readonly ProjectPosDbContext _context;
    private readonly ILogger<SalaryStructureService> _logger;
    private readonly IMapper _mapper;

    public SalaryStructureService(
        ProjectPosDbContext context,
        ILogger<SalaryStructureService> logger,
        IMapper mapper
        ) 
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<SalaryStructureDto>> CreateSalaryStructureAsync(SalaryStructureDto salaryStructureDto)
    {
        try
        {
            var salaryStructure = _mapper.Map<SalaryStructure>(salaryStructureDto);
            salaryStructure.NetSalary = CalculateNetSalary(salaryStructure);

            var employeeDetails = await _context.EmployeeDetails!.FirstOrDefaultAsync(e => e.EmployeeId == salaryStructure.EmployeeId);

            if (employeeDetails == null)
                return ServiceResponse<SalaryStructureDto>.Failure("Employee not found");

            if (!employeeDetails.IsActive || employeeDetails.IsDeleted )
                return ServiceResponse<SalaryStructureDto>.Failure("Employee is Not Active");

            var newSalaryStructure = await _context.SalaryStructures.AddAsync(salaryStructure);

            await _context.SaveChangesAsync();
            
            var salary = await _context.SalaryStructures
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(e => e.Id == newSalaryStructure.Entity.Id);

            return new ServiceResponse<SalaryStructureDto> 
            { 
                Data = _mapper.Map<SalaryStructureDto>(salary),
                Message = "Salary Structure created successfully",
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create Salary Structure" + ex, ex.Message);
            return new ServiceResponse<SalaryStructureDto> 
            { 
                Message = ex.Message, 
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    public async Task<ServiceResponse<SalaryStructureDto>> DeleteSalaryStructureAsync(int userId, int id)
    {
        try
        {
            var salaryStructure = _context.SalaryStructures.Find(id);
            if (salaryStructure == null)
            {
                return new ServiceResponse<SalaryStructureDto> 
                { 
                    Message = "Salary Structure not found", 
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                };
            }

            salaryStructure.IsDeleted = true;
            salaryStructure.DeletionTime = DateTime.UtcNow;
            salaryStructure.DeleterId = userId;

            _context.SalaryStructures.Update(salaryStructure);
            await _context.SaveChangesAsync();

            return new ServiceResponse<SalaryStructureDto> 
            { 
                Message = "Salary Structure deleted successfully", 
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to delete Salary Structure" + ex, ex.Message);
            return new ServiceResponse<SalaryStructureDto> 
            { 
                Message = ex.Message, 
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    public async Task<ServiceResponse<List<SalaryStructureDto>>> GetAllSalaryStructuresAsync()
    {
        try
        {
            var salaryStructures = await _context.SalaryStructures
                .Include(s => s.Employee)
                .ToListAsync();

            return new ServiceResponse<List<SalaryStructureDto>> 
            { 
                Data = _mapper.Map<List<SalaryStructureDto>>(salaryStructures),
                Message = "All Salary Structures retrieved successfully",
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to retrieve all Salary Structures" + ex, ex.Message);
            return new ServiceResponse<List<SalaryStructureDto>> 
            { 
                Message = ex.Message, 
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    public async Task<ServiceResponse<SalaryStructureDto>> GetSalaryStructureByIdAsync(int id)
    {
        try
        {
            var salaryStructure = await _context.SalaryStructures
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salaryStructure == null)
            {
                return new ServiceResponse<SalaryStructureDto> 
                { 
                    Message = "Salary Structure not found", 
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                };
            }

            return new ServiceResponse<SalaryStructureDto> 
            { 
                Data = _mapper.Map<SalaryStructureDto>(salaryStructure),
                Message = "Salary Structure retrieved successfully",
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to retrieve Salary Structure" + ex, ex.Message);
            return new ServiceResponse<SalaryStructureDto> 
            { 
                Message = ex.Message, 
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    public async Task<ServiceResponse<List<SalaryStructureDto>>> GetSalaryStructuresByEmployeeIdAsync(int employeeId)
    {
        try
        {
            var salaryStructures = await _context.SalaryStructures
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);

            // if salaryStructures is null or empty
            if (salaryStructures == null)
            {
                return new ServiceResponse<List<SalaryStructureDto>> 
                { 
                    Message = "Salary Structures not found", 
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                };
            }

            return new ServiceResponse<List<SalaryStructureDto>> 
            { 
                Data = _mapper.Map<List<SalaryStructureDto>>(salaryStructures),
                Message = "Salary Structures retrieved successfully",
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to retrieve Salary Structures" + ex, ex.Message);
            return new ServiceResponse<List<SalaryStructureDto>> 
            { 
                Message = ex.Message, 
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    public async Task<ServiceResponse<SalaryStructureDto>> UpdateSalaryStructureAsync(SalaryStructureDto salaryStructureDto)
    {
        try
        {
            var salaryStructure = _mapper.Map<SalaryStructure>(salaryStructureDto);
            salaryStructure.NetSalary = CalculateNetSalary(salaryStructure);
            var newSalaryStructure = _context.SalaryStructures.Update(salaryStructure);

            await _context.SaveChangesAsync();
            
            var salary = await _context.SalaryStructures
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(e => e.Id == newSalaryStructure.Entity.Id);

            return new ServiceResponse<SalaryStructureDto>
            {
                Data = _mapper.Map<SalaryStructureDto>(salary),
                Message = "Salary Structure updated successfully",
                IsSuccess = true,
                Time = DateTime.UtcNow,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update Salary Structure" + ex, ex.Message);
            return new ServiceResponse<SalaryStructureDto>
            {
                Message = ex.Message,
                IsSuccess = false,
                Time = DateTime.UtcNow,
            };
        }
    }

    private decimal CalculateNetSalary(SalaryStructure salaryStructure)
    {
        // Calculate total allowances
        decimal totalAllowances = (salaryStructure.HousingAllowance) +
                                (salaryStructure.TransportAllowance) +
                                (salaryStructure.OtherAllowance);

        // Calculate total benefits
        decimal totalBenefits = (salaryStructure.MedicalBenefit) +
                                (salaryStructure.PensionBenefit) +
                                (salaryStructure.OtherBenefit);

        // Calculate total deductions
        decimal totalDeductions = (salaryStructure.TaxDeduction) +
                                (salaryStructure.PensionDeduction) +
                                (salaryStructure.AidsLevyDeduction) +
                                (salaryStructure.OtherDeduction);

        // Calculate gross salary (basic + allowances + benefits)
        decimal grossSalary = salaryStructure.BasicSalary + totalAllowances + totalBenefits;

        // Calculate net salary (gross - deductions)
        decimal netSalary = grossSalary - totalDeductions;

        return netSalary;
    }
}