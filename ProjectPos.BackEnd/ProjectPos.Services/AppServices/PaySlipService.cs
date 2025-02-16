using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class PaySlipService : IPaySlipService
{
    private readonly ProjectPosDbContext _context;
    private readonly ILogger<PaySlipService> _logger;
    private readonly IMapper _mapper;

    public PaySlipService(
        ProjectPosDbContext context,
        ILogger<PaySlipService> logger,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ServiceResponse<PayRollCycleDto>> ApprovePayRollAsync(int month, int year, int userId)
    {
        try
        {
            // Input validation
            if (month < 1 || month > 12)
                return ServiceResponse<PayRollCycleDto>.Failure("Invalid month specified");
            
            if (year < 2000 || year > DateTime.Now.Year)
                return ServiceResponse<PayRollCycleDto>.Failure("Invalid year specified");

            // Use a single query with necessary includes
            var payRollCycle = await _context.PayRollCycles!
                .Include(p => p.PaySlips)
                .FirstOrDefaultAsync(p => p.Month == month && p.Year == year);

            if (payRollCycle == null)
                return ServiceResponse<PayRollCycleDto>.Failure("No payroll cycle found for the specified month and year.");

            if (payRollCycle.PayRollStatus == PayRollStatus.Approved)
                return ServiceResponse<PayRollCycleDto>.Failure("Payroll has already been approved.");

            // Update payroll cycle
            payRollCycle.PayRollStatus = PayRollStatus.Approved;
            payRollCycle.PaySlips!.ToList().ForEach(p => p.IsApproved = true);
            payRollCycle.LastModificationTime = DateTime.Now;
            payRollCycle.LastModifierUserId = userId;

            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<PayRollCycleDto>(payRollCycle);
            return ServiceResponse<PayRollCycleDto>.Success(mappedResult, "Payroll approved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to approve payroll for {Month}/{Year}", month, year);
            return ServiceResponse<PayRollCycleDto>.Failure($"Payroll approval failed: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<PaySlipDto>> EditPaySlipAsync(PaySlipDto paySlipDto)
    {
        try
        {
            if (paySlipDto == null)
                return ServiceResponse<PaySlipDto>.Failure("Pay slip data cannot be null");

            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            _context.PaySlips!.Update(paySlip);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<PaySlipDto>(paySlip);
            return ServiceResponse<PaySlipDto>.Success(mappedResult, "Pay slip updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update pay slip");
            return ServiceResponse<PaySlipDto>.Failure($"Pay slip update failed: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<PayRollCycleDto>> GeneratePayRollAsync(int userId)
    {
        try
        {
            var currentDate = DateTime.Now;
            var payPeriod = await GetOrCreatePayPeriod(currentDate, userId);
            if (!payPeriod.IsSuccess)
                return ServiceResponse<PayRollCycleDto>.Failure(payPeriod.Message);

            var payRollCycle = payPeriod.Data;
            var startDate = payRollCycle.StartDate.Date;
            var endDate = currentDate.Date;

            // Get all active employees with their salary structures
            var employees = await _context.Employees!
                .Include(e => e.SalaryStructure)
                .Include(e => e.EmployeeDetails)
                .Include(e => e.Attendances)
                .Include(e => e.OvertimeRecords)
                .Include(e => e.Deductions)
                .Where(e => e.EmployeeDetails != null && e.EmployeeDetails.IsActive)
                .AsSplitQuery()
                .ToListAsync();

            if (!employees.Any())
            {
                return ServiceResponse<PayRollCycleDto>.Failure("No active employees found for payroll generation.");
            }

            var paySlips = new List<PaySlip>();

            foreach (var employee in employees)
            {
                // Filter the included collections for the date range at usage
                var relevantAttendance = employee.Attendances
                    .Where(a => a.Date.Date >= startDate.Date && a.Date <= endDate.Date)
                    .ToList();
                    
                var relevantOvertimeRecords = employee.OvertimeRecords
                    .Where(o => o.Date >= startDate.Date && o.Date.Date <= endDate.Date && o.IsApproved)
                    .ToList();
                    
                var relevantDeductions = employee.Deductions
                    .Where(d => d.DeductionDate.Date >= startDate.Date && d.DeductionDate.Date <= endDate.Date && d.IsApproved == true)
                    .ToList();

                var paySlip = CalculateEmployeePaySlip(
                    employee, 
                    payRollCycle.Id, 
                    startDate, 
                    endDate, 
                    userId,
                    relevantAttendance,
                    relevantOvertimeRecords,
                    relevantDeductions);
                    
                paySlips.Add(paySlip);
            }

            // Update payroll cycle
            payRollCycle.PaySlips = paySlips;
            payRollCycle.IsClosed = true;
            payRollCycle.EndDate = new DateTime(currentDate.Year, currentDate.Month, 26);
            payRollCycle.LastModificationTime = currentDate;
            payRollCycle.LastModifierUserId = userId;

            _context.PayRollCycles!.AddAsync(payRollCycle);
            await _context.SaveChangesAsync();

            var mappedResult = _mapper.Map<PayRollCycleDto>(payRollCycle);
            return ServiceResponse<PayRollCycleDto>.Success(mappedResult, "Payroll generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate payroll");
            return ServiceResponse<PayRollCycleDto>.Failure($"Payroll generation failed: {ex.Message}");
        }
    }

    private async Task<ServiceResponse<PayRollCycle>> GetOrCreatePayPeriod(DateTime currentDate, int userId)
    {
        try
        {
            var existingCycle = await _context.PayRollCycles!
                .FirstOrDefaultAsync(p => p.Month == currentDate.Month && 
                                        p.Year == currentDate.Year);

            if (existingCycle != null && !existingCycle.IsClosed)
                return ServiceResponse<PayRollCycle>.Success(existingCycle);

            if (existingCycle != null)
                return ServiceResponse<PayRollCycle>.Failure("Payroll has already been generated for this period");

            var previousCycle = await _context.PayRollCycles!
                .OrderByDescending(p => p.CreationTime)
                .FirstOrDefaultAsync();
            var start = previousCycle == null ? new DateTime(currentDate.Year, currentDate.Month, 1) : (DateTime)previousCycle!.EndDate.Date.AddDays(1);

            var newCycle = new PayRollCycle
            {
                Month = previousCycle == null ? currentDate.Month : previousCycle!.Month + 1 ,
                Year = currentDate.Year,
                StartDate = start.Date,
                CreationTime = currentDate,
                CreatorId = userId,
                IsClosed = false,
                PayRollStatus = PayRollStatus.Pending
            };

            //await _context.PayRollCycles!.AddAsync(newCycle);
            //await _context.SaveChangesAsync();

            return ServiceResponse<PayRollCycle>.Success(newCycle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get or create pay period for {Date}", currentDate);
            return ServiceResponse<PayRollCycle>.Failure($"Failed to initialize pay period: {ex.Message}");
        }
    }

    private PaySlip CalculateEmployeePaySlip(
        Employee employee,
        int payRollCycleId,
        DateTime startDate,
        DateTime endDate,
        int userId,
        List<Attendance> attendances,
        List<OvertimeRecord> overtimeRecords,
        List<EmployeeDeduction> deductions)
    {
        var salaryStructure = employee.SalaryStructure!;

        // Calculate attendance-based metrics using the passed in collections
        //var workDays = CalculateWorkDays(attendances, startDate, endDate);

        var workDays = CalculateWorkDays(attendances, startDate, endDate);
        var overtimeHours = overtimeRecords.Sum(o => o.Hours);
        
        // Calculate earnings
        decimal basicSalary = CalculateProRatedSalary(salaryStructure.BasicSalary, workDays.ActualDays, workDays.RequiredDays);
        decimal overtimePay = CalculateOvertimePay(salaryStructure.BasicSalary, overtimeHours);
        decimal allowances = CalculateAllowances(salaryStructure, workDays.ActualDays, workDays.RequiredDays);

        // Calculate deductions
        decimal taxDeduction = CalculateTax(basicSalary + overtimePay + allowances);
        decimal pensionDeduction = CalculatePension(basicSalary);
        decimal otherDeductions = deductions.Sum(d => d.Amount);

        decimal totalEarnings = basicSalary + overtimePay + allowances;
        decimal totalDeductions = taxDeduction + pensionDeduction + otherDeductions;

        return new PaySlip
        {
            EmployeeId = employee.Id,
            Month = startDate.Month,
            Year = startDate.Year,
            PayRollCycleId = payRollCycleId,
            GrossSalary = totalEarnings,
            BasicSalary = basicSalary,
            OvertimePay = overtimePay,
            HousingAllowance = salaryStructure.HousingAllowance,
            TransportAllowance = salaryStructure.TransportAllowance,
            OtherAllowance = salaryStructure.OtherAllowance,
            Tax = taxDeduction,
            PensionDeduction = pensionDeduction,
            OtherDeduction = otherDeductions,
            TotalEarning = totalEarnings,
            TotalDeduction = totalDeductions,
            TotalNetSalary = totalEarnings - totalDeductions,
            WorkedDays = workDays.ActualDays,
            OvertimeHours = overtimeHours,
            IsPaid = false,
            IsApproved = false,
            CreationTime = DateTime.Now,
            CreatorId = userId,
            IsPostedToJournal = false,
            PaymentMethod = PaymentMethod.Cash
        };
    }

    private (int ActualDays, int RequiredDays) CalculateWorkDays(
        List<Attendance> attendances,
        DateTime startDate,
        DateTime endDate)
    {
        int actualDays = attendances.Count(a => a.IsPresent);
        int requiredDays = GetBusinessDays(startDate, endDate);

        return (30, 30);
    }

    private int GetBusinessDays(DateTime startDate, DateTime endDate)
    {
        int businessDays = 0;
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                businessDays++;
        }
        return businessDays;
    }

    private async Task<decimal> CalculateOvertimeHours(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _context.OvertimeRecords
            .Where(o => o.EmployeeId == employeeId && 
                       o.Date >= startDate && 
                       o.Date <= endDate &&
                       o.IsApproved)
            .SumAsync(o => o.Hours);
    }

    private decimal CalculateProRatedSalary(decimal fullSalary, int actualDays, int requiredDays)
    {
        return (fullSalary / requiredDays) * actualDays;
    }

    private decimal CalculateOvertimePay(decimal basicSalary, decimal overtimeHours)
    {
        decimal hourlyRate = (basicSalary / 22) / 8; // Assuming 22 working days and 8 hours per day
        return hourlyRate * overtimeHours * 1.5m; // 1.5 times regular rate for overtime
    }

    private decimal CalculateAllowances(SalaryStructure salaryStructure, int actualDays, int requiredDays)
    {
        return CalculateProRatedSalary(
            salaryStructure.HousingAllowance + 
            salaryStructure.TransportAllowance + 
            salaryStructure.OtherAllowance,
            actualDays,
            requiredDays);
    }

    private decimal CalculateTax(decimal grossIncome)
    {
        // Implement tax calculation logic based on your local tax rules
        return grossIncome * 0.0m; // Example: 10% tax rate
    }

    private decimal CalculatePension(decimal basicSalary)
    {
        // Implement pension calculation logic based on your local rules
        return basicSalary * 0.08m; // Example: 8% pension contribution
    }

    private async Task<decimal> CalculateOtherDeductions(int employeeId, DateTime startDate, DateTime endDate)
    {
        // Calculate any advances, loans, or other deductions
        var deductions = await _context.EmployeeDeductions
            .Where(d => d.EmployeeId == employeeId && 
                       d.DeductionDate >= startDate && 
                       d.DeductionDate <= endDate)
            .SumAsync(d => d.Amount);

        return deductions;
    }

    public async Task<ServiceResponse<PayRollCycleDto>> GetPayRollAsync(int month, int year)
    {
        try
        {
            if (month < 1 || month > 12)
                return ServiceResponse<PayRollCycleDto>.Failure("Invalid month specified");
        
            if (year < 2000 || year > DateTime.Now.Year)
                return ServiceResponse<PayRollCycleDto>.Failure("Invalid year specified");

            var payRollCycle = await _context.PayRollCycles!
                .Include(p => p.PaySlips)!
                    .ThenInclude(p => p.Employee)
                .FirstOrDefaultAsync(p => p.Month == month && p.Year == year);

            if (payRollCycle == null)
                return ServiceResponse<PayRollCycleDto>.Failure("No payroll cycle found for the specified month and year.");

            var mappedResult = _mapper.Map<PayRollCycleDto>(payRollCycle);
            return ServiceResponse<PayRollCycleDto>.Success(mappedResult, "Payroll cycle retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve payroll cycle for {Month}/{Year}", month, year);
            return ServiceResponse<PayRollCycleDto>.Failure($"Payroll cycle retrieval failed: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<PaySlipDto>> GetPaySlipAsync(int month, int year, int employeeId)
    {
        try
        {
            // Input validation
            if (month < 1 || month > 12)
                return ServiceResponse<PaySlipDto>.Failure("Invalid month specified");
            
            if (year < 2000 || year > DateTime.Now.Year)
                return ServiceResponse<PaySlipDto>.Failure("Invalid year specified");

            // Use a single efficient query with necessary includes
            var paySlip = await _context.PaySlips!
                .Include(p => p.Employee)
                .Include(p => p.PayRollCycle)
                .FirstOrDefaultAsync(p => 
                    p.Month == month && 
                    p.Year == year && 
                    p.EmployeeId == employeeId);

            if (paySlip == null)
                return ServiceResponse<PaySlipDto>.Failure("No pay slip found for the specified criteria.");

            var mappedResult = _mapper.Map<PaySlipDto>(paySlip);
            return ServiceResponse<PaySlipDto>.Success(mappedResult, "Pay slip retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve pay slip for Employee {EmployeeId} - {Month}/{Year}", 
                employeeId, month, year);
            return ServiceResponse<PaySlipDto>.Failure($"Pay slip retrieval failed: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<PayRollCycleDto>>> GetAllPayRollCycles()
    {
        try
        {
            // Use efficient query with necessary includes
            var payRollCycles = await _context.PayRollCycles!
                .Include(p => p.PaySlips)!
                    .ThenInclude(p => p.Employee)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<PayRollCycleDto>>(payRollCycles);
            return ServiceResponse<List<PayRollCycleDto>>.Success(
                mappedResults, 
                $"Successfully retrieved {mappedResults.Count} payroll cycles.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve payroll cycles");
            return ServiceResponse<List<PayRollCycleDto>>.Failure($"Failed to retrieve payroll cycles: {ex.Message}");
        }
    }

    public async Task<ServiceResponse<List<PayRollCycleDto>>> GetAllPayRollCyclesByYear(int year)
    {
        try
        {
            if (year < 2000 || year > DateTime.Now.Year)
                return ServiceResponse<List<PayRollCycleDto>>.Failure("Invalid year specified");

            var payRollCycles = await _context.PayRollCycles!
                .Include(p => p.PaySlips)!
                    .ThenInclude(p => p.Employee)
                .Where(p => p.Year == year)
                .OrderByDescending(p => p.Month)
                .ToListAsync();

            var mappedResults = _mapper.Map<List<PayRollCycleDto>>(payRollCycles);
            return ServiceResponse<List<PayRollCycleDto>>.Success(
                mappedResults, 
                $"Successfully retrieved {mappedResults.Count} payroll cycles for {year}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve payroll cycles for year {Year}", year);
            return ServiceResponse<List<PayRollCycleDto>>.Failure($"Failed to retrieve payroll cycles: {ex.Message}");
        }
    }
}