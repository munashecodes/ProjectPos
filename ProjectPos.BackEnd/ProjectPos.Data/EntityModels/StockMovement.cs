using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class StockMovement : FullAuditedAggregateRoot<int>
    {
        public int? ProductInventoryId { get; set; }
        public int? BatchNumber { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? TransactionType { get; set; }
        public string? IsssuedTo { get; set; }
        public bool? IsAuthorised { get; set; }
        public int AuthorisedById { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Comment { get; set; }

        [ForeignKey("ProductInventoryId")]
        public ProductInventory? Product { get; set; }

        [ForeignKey("BatchNumber")]
        public StockMovementLog? Batch { get; set; }
    }
}