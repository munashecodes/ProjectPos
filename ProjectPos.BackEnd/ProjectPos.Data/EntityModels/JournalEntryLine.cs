using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Data.EntityModels
{
    public class JournalEntryLine : BasicAggregateRoot<int>
    {
        public int? JournalEntryId { get; set; }
        public int? AccountId { get; set; }
        public decimal? Amount { get; set; } // The debit or credit amount
        public JournalEntryType? Type { get; set; } // "Debit" or "Credit"

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
        [ForeignKey("JournalEntryId")]
        public JournalEntry? JournalEntry { get; set; }
    }
}