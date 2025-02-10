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
    public class StockMovementLog : FullAuditedAggregateRoot<int>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? TransactionType { get; set; }
        public string? IsssuedTo { get; set; }
        public bool? IsAuthorised { get; set; }
        public int AuthorisedById { get; set; }

        //related entities
        public List<StockMovement>? StockMovements { get; set; }
    }
}
