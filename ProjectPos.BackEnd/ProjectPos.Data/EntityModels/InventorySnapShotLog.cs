using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class InventorySnapShotLog : FullAuditedAggregateRoot<int>
    {
        public Category Department { get; set; }
        public DateTime? StartDay { get; set; }
    }
}
