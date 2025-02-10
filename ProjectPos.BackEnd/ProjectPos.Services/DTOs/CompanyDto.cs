using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ProjectPos.Services.DTOs
{
    public class CompanyDto : AuditedEntityDto<int>
    {
        public string? Name { get; set; }
        public int? AddressId { get; set; }
        public int? VatNumber { get; set; }
        public int? RegNumber { get; set; }
        public int? AccountNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? IsSupplier { get; set; }
        public bool? HasCreditFacility { get; set; }

        [ForeignKey("AddressId")]
        public AddressDto? Address { get; set; }
    }
}