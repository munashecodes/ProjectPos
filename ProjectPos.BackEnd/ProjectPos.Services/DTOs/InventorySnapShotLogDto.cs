using ProjectPos.Data.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class InventorySnapShotLogDto : FullAuditedAggregateRoot<int>
    {
        public string? UserName { get; set; }
        public IEnumerable<ProductInventorySnapshotDto>? InventorySnapshots { get; set; }
    }
}
