using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class ExchangeRate : FullAuditedAggregateRoot<int>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? BaseCurrency { get; set; }
        public decimal BaseToRate { get; set; }
        public DateTime DateEffected { get; set; }
    }
}
