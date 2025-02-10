using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Services.DTOs
{
    public class SalesOrderItemDto : EntityDto<int>
    {
        public int? OrderNumber { get; set; }
        public string? ProductName { get; set; }
        public string? BarCode { get; set; }
        public bool? IsTaxable { get; set; }
        public int? ProductId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Quantity { get; set; }
        public Unit? Unit { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
    }
}