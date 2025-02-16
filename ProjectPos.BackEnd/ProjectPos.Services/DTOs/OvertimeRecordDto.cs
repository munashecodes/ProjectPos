namespace ProjectPos.Services.DTOs;

public class OvertimeRecordDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime Date { get; set; }
    public decimal? Hours { get; set; }
    public decimal? Rate { get; set; }
    public decimal? Amount { get; set; }
    public bool IsApproved { get; set; }
    public string? Notes { get; set; }
}