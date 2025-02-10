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
    public class ProofOfPayment : FullAuditedAggregateRoot<int>
    {
        public string? Reference {  get; set; }
        public int? CustomerId { get; set; }
        public string? Bank { get; set;}
        public string? BranchCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? UsableAmount { get; set; }
        public DateTime? BankingDate { get; set; }

        //foreign keys
        [ForeignKey("CustomerId")]
        public Company? Customer { get; set; }
    }
}
