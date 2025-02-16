using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.EntityModels;

public class SalaryStructure: FullAuditedAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public Currency Currency { get; set; }
    public decimal BasicSalary { get; set; }
//allowances
    public decimal HousingAllowance { get; set; } = 0;
    public decimal TransportAllowance { get; set; } = 0;
    public decimal OtherAllowance { get; set; } = 0;
//benefits
    public decimal MedicalBenefit { get; set; } = 0;
    public decimal PensionBenefit { get; set; } = 0;
    public decimal OtherBenefit { get; set; } = 0;
//deductions
    public decimal TaxDeduction { get; set; } = 0;
    public decimal PensionDeduction { get; set; } = 0;
    public decimal AidsLevyDeduction { get; set; } = 0;
    public decimal OtherDeduction { get; set; } = 0;
//over time
    public decimal OvertimeRate { get; set; } = 0;
    public decimal OvertimeHours { get; set; } = 0;
    public decimal OvertimeTotal { get; set; } = 0;
//hourly rate for part time employees
    public decimal HourlyRate { get; set; } = 0;
    public decimal HoursWorked { get; set; } = 0;
//taxable income
    public decimal TaxableIncome { get; set; } = 0;
//net salary
    public decimal NetSalary { get; set; } = 0;
//notes
    public string? Notes { get; set; }

    public Employee? Employee { get; set; }
}