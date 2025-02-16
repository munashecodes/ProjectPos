using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.EntityModels;

public class PaySlip : FullAuditedAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int PayRollCycleId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal? Allowance { get; set; }
    public decimal? TillShortageDeduction { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? Tax { get; set; }
    public decimal? TotalEarning { get; set; }
    public decimal? PensionDeduction { get; set; }
    public decimal? TotalDeduction { get; set; }
    public decimal? TotalNetSalary { get; set; }
    public bool IsPaid { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    public int? ApprovedBy { get; set; }
    public bool IsPostedToJournal { get; set; } = false;
    public decimal OvertimePay { get; set; }
    public decimal HousingAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal OtherAllowance { get; set; }
    public decimal OtherDeduction { get; set; }
    public int WorkedDays { get; set; }
    public decimal OvertimeHours { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
    [ForeignKey("PayRollCycleId")]
    public PayRollCycle? PayRollCycle { get; set; }
    [ForeignKey("ApprovedBy")]
    public User? User { get; set; }
}