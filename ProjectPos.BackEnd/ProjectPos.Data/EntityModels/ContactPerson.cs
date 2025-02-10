using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class ContactPerson : AuditedAggregateRoot<int>
    {
        public int? CompanyId { get; set; }
        public int AddressId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Title? Title { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JobTitle? JobPosition { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }
    }
}