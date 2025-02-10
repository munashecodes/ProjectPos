using ProjectPos.Data.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class JournalEntry : AuditedAggregateRoot<int>
    {
        public string? Description { get; set; }

        public List<JournalEntryLine>? JournalEntryLines { get; set; }
    }
}
