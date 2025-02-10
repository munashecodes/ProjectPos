using ProjectPos.Data.AggregateRoots;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Data.EntityModels
{
    public class ProductPrice : FullAuditedAggregateRoot<int>
    {
        public int? ProductInventoryId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? MarkUp { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("ProductInventoryId")]
        public ProductInventory? Product { get; set; }
    }
}