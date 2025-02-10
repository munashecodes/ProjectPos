using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class ExchangeRateDto : FullAuditedEntityDto<int>
    {
        public Currency? Currency { get; set; }
        public Currency? BaseCurrency { get; set; }
        public decimal BaseToRate { get; set; }
        public DateTime DateEffected { get; set; }
    }
}
