using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.EntityModels;

public class EmployeeDetails : FullAuditedAggregateRoot<int>
{
    public int EmployeeId { get; set; }
    public Department Department { get; set; }
    public string? Position { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string? Bank { get; set; }
    public string? BankAccountNumber { get; set; }
    public SalaryType SalaryType { get; set; }
    public DateTime JoiningDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public bool IsActive { get; set; } = true;

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
}