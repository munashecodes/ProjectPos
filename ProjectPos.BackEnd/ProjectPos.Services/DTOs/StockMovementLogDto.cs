using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class StockMovementLogDto : FullAuditedEntityDto<int>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? TransactionType { get; set; }
        public string? IsssuedTo { get; set; }
        public bool? IsAuthorised { get; set; }
        public int AuthorisedById { get; set; }
        public string? AuthorisedBy { get; set; }
        public string? CreatedBy { get; set; }

        //related entities
        public List<StockMovementDto>? StockMovements { get; set; }
    }
}
