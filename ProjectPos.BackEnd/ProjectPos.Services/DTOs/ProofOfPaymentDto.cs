using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class ProofOfPaymentDto : FullAuditedEntityDto<int>
    {
        public string? Reference { get; set; }
        public int? CustomerId { get; set; }
        public string? Bank { get; set; }
        public string? CustomerName { get; set; }
        public string? CreatorName { get; set; }
        public string? BranchCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? UsableAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? UsedAmount { get; set; }
        public DateTime? BankingDate { get; set; }
    }
}
