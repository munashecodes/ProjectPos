using ProjectPos.Data.EntityModels;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class EmployeeDto : FullAuditedEntityDto<int>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        [Required]
        public string? NatId { get; set; }
        public DateTime? DOB { get; set; }
        public int? AddressId { get; set; }
        [Required]
        public string? Cell { get; set; }
        public string? Email { get; set; }

        [ForeignKey("AddressId")]
        public AddressDto? Address { get; set; }
    }
}
