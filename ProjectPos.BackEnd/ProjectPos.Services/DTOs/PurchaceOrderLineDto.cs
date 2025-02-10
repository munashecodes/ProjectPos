using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class PurchaceOrderLineDto : EntityDto<int>
    {
        public int? OrderNumber { get; set; }
        public int? ProductInventoryId { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Unit? Unit { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? UnitPrice { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? Price { get; set; }

        //foreign Key

        [ForeignKey("ProductInventoryId")]
        public ProductInventoryDto? Product { get; set; }
    }
}