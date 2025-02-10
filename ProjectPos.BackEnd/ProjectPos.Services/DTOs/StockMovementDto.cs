using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class StockMovementDto : FullAuditedEntityDto<int>
    {
        public int? ProductInventoryId { get; set; }
        public string? ProductName { get; set; }
        public string? BarCode { get; set; }
        public int? BatchNumber { get; set; }
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
        public ProductInventoryDto? Product { get; set; }
    }
}