using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectPos.Services.DTOs
{
    public class ContactPersonDto : AuditedEntityDto<int>
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
        public CompanyDto? Company { get; set; }

        [ForeignKey("AddressId")]
        public AddressDto? Address { get; set; }
    }
}