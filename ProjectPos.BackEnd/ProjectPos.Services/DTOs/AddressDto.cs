using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectPos.Services.DTOs
{
    public class AddressDto : EntityDto<int>
    {
        public string? Street { get; set; }
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
    }
}
