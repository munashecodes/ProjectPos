using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class InventorySnapShotSummaryDto
    {
        public int UserId { get; set; }
        public SnapShotEnum? SnapShotType { get; set; }
        public Category? Department { get; set; }
    }
}
