using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class PurchaceOrder : FullAuditedAggregateRoot<int>
    {
        [Column(TypeName = "double(12, 2)")]
        public double? InvoiceValue { get; set; }
        public bool IsReceived { get; set; }
        public bool IsOpen { get; set; }
        public int? SupplierId { get; set; }
        public bool IsApproved { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime ETA { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderReceivedStatus? Status { get; set; } = OrderReceivedStatus.NotReceived;

        //Foreign Keys
        [ForeignKey("SupplierId")]
        public Company? Company { get; set; }

        //dependent entities
        public List<PurchaceOrderLine>? PurchaceOrderItems { get; set; }
        public ICollection<GoodsReceivedVoucher>? GoodsReceivedVouchers { get; set; }
    }
}
