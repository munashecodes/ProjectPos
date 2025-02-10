using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class PurchaceInvoiceDto : FullAuditedEntityDto<int>
    {
        public int InvoiceNumber { get; set; }
        public string? Supplier { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double TotalValue { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? TransactionType { get; set; }
        public DateTime Date { get; set; }

        public ICollection<PurchaceInvoiceLineDto>? InvoiceLines { get; set; }
    }
}
