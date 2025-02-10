using ProjectPos.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.AggregateRoots
{
    public class AuditedAggregateRoot<T> : BasicAggregateRoot<T>
    {
        public DateTime CreationTime { get; set; }
        public int? CreatorId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionTime { get; set; }
        public int? DeleterId { get; set; }

        [ForeignKey("CreatorId")]
        public User? Creator { get; set; }
        [ForeignKey("DeleterId")]
        public User? Deleter { get; set; }

        public void PrepareEntityForCreate()
        {
            CreationTime = DateTime.Now;
        }
    }
}
