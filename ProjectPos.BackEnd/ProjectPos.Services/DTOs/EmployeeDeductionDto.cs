namespace ProjectPos.Services.DTOs;

public class EmployeeDeductionDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime DeductionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Reason { get; set; }
    public bool IsApproved { get; set; }
    public string? Notes { get; set; }
}