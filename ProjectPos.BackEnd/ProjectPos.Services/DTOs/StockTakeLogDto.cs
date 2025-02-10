using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class StockTakeLogDto : AuditedEntityDto<int>
    {
        public Category Department { get; set; }
    }
}
