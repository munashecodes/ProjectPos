using ProjectPos.Data.AggregateRoots;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace ProjectPos.Data.EntityModels
{
    public class Company : AuditedAggregateRoot<int>
    {
        public string? Name { get; set; }
        public int AddressId { get; set; }
        public int VatNumber { get; set; }
        public int RegNumber { get; set; }
        public int AccountNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsSupplier { get; set; }
        public bool HasCreditFacility { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

        public ICollection<ContactPerson>? ContactPersons { get; set; }
    }
}