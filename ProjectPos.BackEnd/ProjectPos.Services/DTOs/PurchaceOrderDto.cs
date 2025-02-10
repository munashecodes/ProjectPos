using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class PurchaceOrderDto : FullAuditedEntityDto<int>
    {
        [Column(TypeName = "double(12, 2)")]
        public double? InvoiceValue { get; set; }
        public string? SupplierName { get; set; }
        public bool IsReceived { get; set; }
        public bool IsOpen { get; set; }
        public int? SupplierId { get; set; }
        public bool IsApproved { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime ETA { get; set; }
        public OrderReceivedStatus? Status { get; set; }

        //Foreign Keys
        [ForeignKey("SupplierId")]
        public CompanyDto? Company { get; set; }

        //dependent entities
        public List<PurchaceOrderLineDto>? PurchaceOrderItems { get; set; }
    }
}
