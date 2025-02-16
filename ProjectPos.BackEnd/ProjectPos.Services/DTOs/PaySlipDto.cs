using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;

namespace ProjectPos.Services.DTOs;

public class PaySlipDto : FullAuditedEntityDto<int>
{
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeSurname { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal? Allowance { get; set; }
    public decimal? TillShortageDeduction { get; set; }
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? Tax { get; set; }
    public decimal? TotalEarning { get; set; }
    public decimal? TotalDeduction { get; set; }
    public decimal? TotalNetSalary { get; set; }
    public bool IsPaid { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    public int? ApprovedBy { get; set; }
    public bool IsPostedToJournal { get; set; } = false;
}