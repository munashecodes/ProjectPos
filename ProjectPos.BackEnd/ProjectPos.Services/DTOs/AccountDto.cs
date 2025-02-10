using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class AccountDto : AuditedEntityDto<int>
    {
        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Balance { get; set; }
        public AccountType? AccountType { get; set; }
        public int? AccountCategoryId { get; set; }


        public ICollection<JournalEntryLineDto>? JournalEntryLines { get; set; }
        
    }
}
