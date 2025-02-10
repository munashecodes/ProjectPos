using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class GoodsReceivedVoucherLine : BasicAggregateRoot<int>
    {
        public int? VoucherNumber { get; set; }
        public int? ProductInventoryId { get; set; }
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
        public ProductInventory? Product { get; set; }

        [ForeignKey("VoucherNumber")]
        public GoodsReceivedVoucher? GoodsReceivedVoucher { get; set; }
    }
}