using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;

namespace ProjectPos.Data.EntityModels;

public class Attendance: BasicAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee? Employee { get; set; }
}