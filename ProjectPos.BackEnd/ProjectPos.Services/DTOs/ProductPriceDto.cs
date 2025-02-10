using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Services.DTOs
{
    public class ProductPriceDto : FullAuditedEntityDto<int>
    {
        public int? ProductInventoryId { get; set; }
        public string? BarCode { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? MarkUp { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("ProductInventoryId")]
        public ProductInventoryDto? Product { get; set; }
    }
}