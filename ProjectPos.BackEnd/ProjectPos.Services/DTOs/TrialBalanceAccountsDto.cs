using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Services.DTOs;

public class TrialBalanceAccountsDto
{
    public string? PastelRef { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? DebitBalance { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? CreditBalance { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AccountType? AccountType { get; set; }
    public int? AccountCategoryId { get; set; }
    public string? AccountCategoryName { get; set; }
}