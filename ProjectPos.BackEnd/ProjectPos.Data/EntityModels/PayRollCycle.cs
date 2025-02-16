using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.EntityModels;

public class PayRollCycle: FullAuditedAggregateRoot<int>
{
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; } = false;
    public decimal TotalGrossSalary { get; set; } = 0;
    public decimal TotalTax { get; set; } = 0;
    public decimal TotalPensionDeduction { get; set; } = 0;
    public decimal TotalOtherDeduction { get; set; } = 0;
    public decimal TotalAllowance { get; set; } = 0;
    public decimal TotalBasic { get; set; } = 0;
    public decimal TotalNetSalary { get; set; } = 0;
    public PayRollStatus PayRollStatus { get; set; } = PayRollStatus.Pending;

    public List<PaySlip>? PaySlips { get; set; }
}