using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class GetExchangeRatesListDto
    {
        public Currency? Currency { get; set; }
        public IEnumerable<ExchangeRateDto>? ExchangeRates { get; set; }
    }
}
