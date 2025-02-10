using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class GoodsReceivedVoucherLineDto : EntityDto<int>
    {
        public int? VoucherNumber { get; set; }
        public int? ProductInventoryId { get; set; }
        public string? BarCode { get; set; }
        public string? ProductName { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? OrderedQuantity { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ReceivedQuantity { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? IssuedQuantity { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QtyOnHand { get; set; }
        public bool? IsIssued { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Unit? Unit { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? OrderPrice { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? UnitPrice { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? Price { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        //foreign keys
        [ForeignKey("ProductInventoryId")]
        public ProductInventoryDto? Product { get; set; }
    }
}