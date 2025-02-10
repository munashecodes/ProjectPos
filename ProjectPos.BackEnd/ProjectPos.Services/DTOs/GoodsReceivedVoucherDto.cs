using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class GoodsReceivedVoucherDto : FullAuditedEntityDto<int>
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
        public string? SupplierName { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double? Value { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderPaymentStatus? Status { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        [ForeignKey("SupplierId")]
        public CompanyDto? Supplier { get; set; }

        //dependent entities
        public List<GoodsReceivedVoucherLineDto>? ReceivedItems { get; set; }
        public List<PurchaceOrderPaymentsDto>? PurchaceOrderPayments { get; set; }
    }
}