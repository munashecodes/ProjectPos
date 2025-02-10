using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IExchangeRateService
    {
        public ServiceResponse<ExchangeRateDto> Create(ExchangeRateDto rate);
        public ServiceResponse<ExchangeRateDto> Update(ExchangeRateDto rate);
        public ServiceResponse<ExchangeRateDto> Delete(int id);
        public ServiceResponse<ExchangeRateDto> Get(int id);
        public ServiceResponse<List<GetExchangeRateDto>> GetAll();
        public ServiceResponse<IEnumerable<GetExchangeRatesListDto>> GetByDate(DateTime date);
    }
}
