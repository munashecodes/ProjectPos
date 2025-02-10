using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class ProductInventorySnapshot : FullAuditedAggregateRoot<int>
    {
        public string? Inventory { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category? Department { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SnapShotEnum? SnapShotType { get; set; }

    }
}