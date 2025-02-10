using ProjectPos.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.AggregateRoots
{
    public class FullAuditedAggregateRoot<T> : AuditedAggregateRoot<T>
    {
        public DateTime? LastModificationTime { get; set; }
        public int? LastModifierUserId { get; set; } = null;

        [ForeignKey("LastModifierUserId")]
        public User? LastModifierUser { get; set; }
    }
}
