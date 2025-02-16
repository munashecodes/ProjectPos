using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;

namespace ProjectPos.Data.EntityModels;

public class EmployeeDeduction : BasicAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public DateTime DeductionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public bool IsApproved { get; set; }
    public string? Notes { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee? Employee { get; set; }
}