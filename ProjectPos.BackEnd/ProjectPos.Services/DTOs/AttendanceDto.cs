namespace ProjectPos.Services.DTOs;

public class AttendanceDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}