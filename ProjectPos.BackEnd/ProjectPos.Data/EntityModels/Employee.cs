using ProjectPos.Data.AggregateRoots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class Employee : FullAuditedAggregateRoot<int>
    {
        public string? Name { get; set; } = null!;
        public string? Surname { get; set; } = null!;
        public string? NatId { get; set; }
        public DateTime? DOB { get; set; }
        public int? AddressId { get; set; }
        public string? Cell { get; set; }
        public string? Email { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

    }
}
