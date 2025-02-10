using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class GoodsReceivedVoucher : FullAuditedAggregateRoot<int>
    {
        public int? OrderNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Transpoter { get; set; }
        public int? SupplierId { get; set; }
        [Column(TypeName = "double(12, 2)")]
        public double? PaidAmount { get; set; }
        [Column(TypeName = "double(12, 2)")]
        public double? UsdPaidAmount { get; set; }
        [Column(TypeName = "double(12, 2)")]
        public double? AmountDue { get; set; }
        [Column(TypeName = "double(12, 2)")]
        public double? Value { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderPaymentStatus? Status { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        //foreign Keys
        [ForeignKey("OrderNumber")]
        public PurchaceOrder? PurchaceOrder { get; set; }

        [ForeignKey("SupplierId")]
        public Company? Supplier { get; set; }

        //dependent entities
        public List<GoodsReceivedVoucherLine>? ReceivedItems { get; set; }
        public List<PurchaceOrderPayments>? PurchaceOrderPayments { get; set; }
    }
}