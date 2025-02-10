using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.EntityDtos
{
    public class FullAuditedEntityDto<T> : AuditedEntityDto<T>
    {
        public DateTime? LastModificationTime { get; set; }
        public int? LastModifierUserId { get; set; } = null;
    }
}
