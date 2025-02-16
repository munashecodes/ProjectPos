using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class Account : AuditedAggregateRoot<int>
    {
        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Balance { get; set; } = 0;
        public AccountType? AccountType { get; set; }
        public int? AccountCategoryId { get; set; }

        [ForeignKey("AccountCategoryId")]
        public AccountCategory? AccountCategory { get; set; }

        public ICollection<JournalEntryLine>? JournalEntryLines { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
