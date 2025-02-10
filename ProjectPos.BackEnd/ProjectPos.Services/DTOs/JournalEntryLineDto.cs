using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Services.DTOs
{
    public class JournalEntryLineDto : EntityDto<int>
    {
        public int? JournalEntryId { get; set; }
        public int? AccountId { get; set; }
        public decimal? Amount { get; set; } // The debit or credit amount
        public JournalEntryType? Type { get; set; } // "Debit" or "Credit"

    }
}