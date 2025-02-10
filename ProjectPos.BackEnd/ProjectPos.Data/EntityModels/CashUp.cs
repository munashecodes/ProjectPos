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
    public class CashUp : FullAuditedAggregateRoot<int>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Rate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
        public decimal? USDAmount { get; set; }
    }
}
