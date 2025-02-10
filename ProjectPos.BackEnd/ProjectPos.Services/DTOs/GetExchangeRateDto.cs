using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class GetExchangeRateDto
    {
        public Currency? Currency { get; set; }
        public ExchangeRateDto? ExchangeRate { get; set; }
        public DateTime DateEffected { get; set; }
    }
}
