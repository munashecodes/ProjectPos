using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.EntityDtos
{
    public class AuditedEntityDto<T> : EntityDto<T>
    {
        public DateTime CreationTime { get; set; }
        public int? CreatorId { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionTime { get; set; }
        public int? DeleterId { get; set; } = null;
    }
}
