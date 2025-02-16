using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;

namespace ProjectPos.Services.DTOs;

public class GroupedGrvItemsDto : EntityDto<int>
{
    public Category? Category { get; set; }
    public string? SubCategory { get; set; }
    public int? SubCategoryId { get; set; }
    public int? VoucherNumber { get; set; }
    public string? ProductName { get; set; }
    public string? BarCode { get; set; }
    public int? ProductId { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? UnitCost { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? Quantity { get; set; }
    public Unit? Unit { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? TotalCost { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? OpeningStock { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? ClosingStock { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? OpeningQuantity { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? ClosingQuantity { get; set; }
}