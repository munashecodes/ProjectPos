using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;

namespace ProjectPos.Data.EntityModels;

public class OvertimeRecord: BasicAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public bool IsApproved { get; set; }
    public int? ApprovedById { get; set; }
    public string? Notes { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee? Employee { get; set; }
    [ForeignKey("ApprovedById")]
    public virtual User? ApprovedBy { get; set; }
}