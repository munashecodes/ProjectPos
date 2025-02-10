using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectPos.Services.AppServices
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        public ExchangeRateService(ProjectPosDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<ExchangeRateDto> Create(ExchangeRateDto rate)
        {
            try
            {
                rate.DateEffected = DateTime.Today;
                var _rate = _mapper.Map<ExchangeRateDto, ExchangeRate>(rate);
                var res = _context.ExchangeRates.Add(_rate);
                _context.SaveChanges();

                return new ServiceResponse<ExchangeRateDto>
                {
                    Data = _mapper.Map<ExchangeRate, ExchangeRateDto>(res.Entity),
                    Message = "ExchangeRate Created Successfully",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ExchangeRateDto>
                {
                    Message = $"ExchangeRate Not Created {ex.Message}",
                    Time = DateTime.Now,
                    IsSuccess = false
                };
            }
        }

        public ServiceResponse<ExchangeRateDto> Delete(int id)
        {
            try
            {
                var _rate = _context.ExchangeRates.FirstOrDefault(x => x.Id == id);
                if (_rate != null)
                {
                    var res = _context.ExchangeRates.Remove(_rate);
                    _context.SaveChanges();
                    return new ServiceResponse<ExchangeRateDto>
                    {
                        Message = $"{res.Entity.Currency} was deleted successfully",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<ExchangeRateDto>
                    {
                        Message = $"ExchangeRate with Id {id} was not found",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ExchangeRateDto>
                {
                    Message = $"ExchangeRate Not Deleted {ex.Message}",
                    Time = DateTime.Now,
                    IsSuccess = false
                };
            }
        }

        public ServiceResponse<ExchangeRateDto> Get(int id)
        {
            try
            {
                var _rate = _context.ExchangeRates.FirstOrDefault(x => x.Id == id);
                if (_rate != null)
                {
                    var res = _mapper.Map<ExchangeRate, ExchangeRateDto>(_rate);
                    return new ServiceResponse<ExchangeRateDto>
                    {
                        Data = res,
                        Message = $"{res.Currency} Found",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<ExchangeRateDto>
                    {
                        Message = $"Exchange Rate with Id {id} was not found",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<IEnumerable<GetExchangeRatesListDto>> GetByDate(DateTime date)
        {
            try
            {
                var rate = _context.ExchangeRates
                    .Where(x => x.CreationTime.Date == date.Date)
                    .OrderByDescending(x => x.DateEffected)
                    .ToList();

                var rateDtos = _mapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(rate);

                var rates = rateDtos
                    .GroupBy(x => x.Currency)
                    .Select(x => new GetExchangeRatesListDto
                    {
                        Currency = x.Key,
                        ExchangeRates = x.OrderByDescending(z => z.DateEffected),
                    });

                return new ServiceResponse<IEnumerable<GetExchangeRatesListDto>>
                {
                    Data = rates,
                    Message = $"Retreived {rateDtos.Count} Found",
                    Time = DateTime.Now,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<List<GetExchangeRateDto>> GetAll()
        {
            try
            {
                var rate = _context.ExchangeRates
                    .Where(x => x.DateEffected < DateTime.Now || x.DateEffected == DateTime.Today)
                    .OrderByDescending(x => x.DateEffected)
                    .ToList();

                var rateDtos = _mapper.Map<List<ExchangeRate>, List<ExchangeRateDto>>(rate);


                var rates = rateDtos
                    .GroupBy(x => x.Currency)
                    .Select(x => new GetExchangeRateDto
                    {
                        Currency = x.Key,
                        ExchangeRate = x.MaxBy(z => z.DateEffected),
                        DateEffected = x.Max(z => z.DateEffected),
                    })
                    .ToList();

                return new ServiceResponse<List<GetExchangeRateDto>>
                {
                    Data = rates,
                    Message = $"Retrieved {rates.Count} records",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetExchangeRateDto>>
                {
                    Message = $"{ex.Message}",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
        }

        public ServiceResponse<ExchangeRateDto> Update(ExchangeRateDto rate)
        {
            try
            {
                var _rate = _mapper.Map<ExchangeRateDto, ExchangeRate>(rate);
                var res = _context.ExchangeRates.Update(_rate);
                _context.SaveChanges();

                return new ServiceResponse<ExchangeRateDto>
                {
                    Data = _mapper.Map<ExchangeRate, ExchangeRateDto>(res.Entity),
                    Message = "ExchangeRate Updated Successfully",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
