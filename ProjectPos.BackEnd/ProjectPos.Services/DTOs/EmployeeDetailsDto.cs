using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Services.DTOs;

public class EmployeeDetailsDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Department Department { get; set; }
    public string? Position { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string? Bank { get; set; }
    public string? BankAccountNumber { get; set; }
    public SalaryType SalaryType { get; set; }
    public DateTime JoiningDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public bool IsActive { get; set; } = true;
}