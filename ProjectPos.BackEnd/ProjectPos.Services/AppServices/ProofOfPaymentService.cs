using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using ProjectPos.Services.ReportingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectPos.Services.AppServices
{
    public class ProofOfPaymentService : IProofOfPaymentService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProofOfPaymentService> _logger;

        public ProofOfPaymentService(
            ProjectPosDbContext context, 
            IMapper mapper,
            ILogger<ProofOfPaymentService> logger
            )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<ProofOfPaymentDto> Create(ProofOfPaymentDto proofOfPayment)
        {
            try
            {
                var _proofOfPayment = _mapper.Map<ProofOfPaymentDto, ProofOfPayment>(proofOfPayment);
                var res = _context.ProofOfPayments.Add(_proofOfPayment);
                _context.SaveChanges();

                return new ServiceResponse<ProofOfPaymentDto>
                {
                    Data = _mapper.Map<ProofOfPayment, ProofOfPaymentDto>(res.Entity),
                    Message = "ProofOfPayment Created Successfully",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<ProofOfPaymentDto> Delete(int id)
        {
            try
            {
                var _proofOfPayment = _context.ProofOfPayments.FirstOrDefault(x => x.Id == id);
                if (_proofOfPayment != null)
                {
                    var res = _context.ProofOfPayments.Remove(_proofOfPayment);
                    _context.SaveChanges();
                    return new ServiceResponse<ProofOfPaymentDto>
                    {
                        Message = $"{res.Entity.Id} was deleted successfully",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<ProofOfPaymentDto>
                    {
                        Message = $"ProofOfPayment with Id {id} was not found",
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

        public ServiceResponse<ProofOfPaymentDto> Get(int id)
        {
            try
            {
                var _proofOfPayment = _context.ProofOfPayments.FirstOrDefault(x => x.Id == id);
                if (_proofOfPayment != null)
                {
                    var res = _mapper.Map<ProofOfPayment, ProofOfPaymentDto>(_proofOfPayment);
                    return new ServiceResponse<ProofOfPaymentDto>
                    {
                        Data = res,
                        Message = $"{res.Id} Found",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<ProofOfPaymentDto>
                    {
                        Message = $"ProofOfPayment with Id {id} was not found",
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

        public ServiceResponse<List<ProofOfPaymentDto>> GetAll()
        {
            try
            {
                var proofOfPayment = _context.ProofOfPayments!
                    .Include(x => x.Customer)
                    .ToList();
                var ProofOfPaymentDtos = _mapper.Map<List<ProofOfPayment>, List<ProofOfPaymentDto>>(proofOfPayment);

                return new ServiceResponse<List<ProofOfPaymentDto>>
                {
                    Data = ProofOfPaymentDtos,
                    Message = $"Retreived {ProofOfPaymentDtos.Count} Found",
                    Time = DateTime.Now,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAll(DateTime date)
        {
            try
            {
                var payments = _context.ProofOfPayments!
                    .Include(x => x.Customer!)
                    .Where(x => x.CreationTime.Day == date.Day)
                    .ToList();

                var _payments = _mapper.Map<IEnumerable<ProofOfPayment>, IEnumerable<ProofOfPaymentDto>>(payments);

                foreach (var item in _payments)
                {
                    item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.UsedAmount = item.PaidAmount - item.UsableAmount;
                }

                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    Data = _payments,
                    IsSuccess = true,
                    Message = $"Found  Payments",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all payments");
                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAll(DateTime startDate, DateTime endDate)
        {
            try
            {
                var payments = _context.ProofOfPayments!
                .Include(x => x.Customer!)
                    .Where(x =>
                        x.CreationTime >= startDate
                        && x.CreationTime <= endDate)
                    .ToList();

                var _payments = _mapper.Map<IEnumerable<ProofOfPayment>, IEnumerable<ProofOfPaymentDto>>(payments);

                foreach (var item in _payments)
                {
                    item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.UsedAmount = item.PaidAmount - item.UsableAmount;
                }

                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    Data = _payments,
                    IsSuccess = true,
                    Message = $"Found  Payments",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all payments");
                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ProofOfPaymentDto>> GetAllByCustomerId(int id)
        {
            try
            {
                var proofOfPayment = _context.ProofOfPayments!
                    .Where(x => x.CustomerId == id)
                    .ToList();
                var ProofOfPaymentDtos = _mapper.Map<List<ProofOfPayment>, List<ProofOfPaymentDto>>(proofOfPayment);

                return new ServiceResponse<List<ProofOfPaymentDto>>
                {
                    Data = ProofOfPaymentDtos,
                    Message = $"Retreived {ProofOfPaymentDtos.Count} Found",
                    Time = DateTime.Now,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetDateReportByCustomer(DateTime date)
        {
            try
            {
                date = date.AddDays(1);

                var sales = _context.ProofOfPayments!
                    .Include(x => x.Customer!)
                    .Where(x => x.CreationTime.Date == date.Date)
                    .ToList();

                if (sales.Any())
                {
                    var _sales = _mapper.Map<List<ProofOfPayment>, List<ProofOfPaymentDto>>(sales);

                    foreach (var item in _sales)
                    {
                        item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.UsedAmount = item.PaidAmount - item.UsableAmount;
                    }

                    var groupedSales = _sales
                    .GroupBy(x => x.CustomerId)
                    .Select(x => new ProofOfPaymentReportDto
                    {
                        CustomerId = x.Key,
                        ProofOfPayments = x.OrderByDescending(z => z.CreationTime).ToList(),
                        PaidTotal = x.ToList().Sum(z => z.PaidAmount),
                        CustomerName = _context.Companies!.FirstOrDefault(u => u.Id == x.Key)!.Name,
                        UsableTotal = x.ToList().Sum(z => z.UsableAmount),
                        UsedTotal = x.ToList().Sum(z => z.PaidAmount) - x.ToList().Sum(z => z.UsableAmount),
                    });

                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        Data = groupedSales,
                        IsSuccess = true,
                        Message = $"",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _logger.LogError("no sales where found for this day");
                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        IsSuccess = false,
                        Message = $"no sales where found for this day",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAllMonth(int month)
        {
            try
            {
                var payments = _context.ProofOfPayments!
                .Include(x => x.Customer!)
                    .Where(x => x.CreationTime.Month == month)
                    .ToList();

                var _payments = _mapper.Map<IEnumerable<ProofOfPayment>, IEnumerable<ProofOfPaymentDto>>(payments);

                foreach (var item in _payments)
                {
                    item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.UsedAmount = item.PaidAmount - item.UsableAmount;
                }

                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    Data = _payments,
                    IsSuccess = true,
                    Message = $"Found  Payments",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all payments");
                return new ServiceResponse<IEnumerable<ProofOfPaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetRangeReportByCustomer(DateTime startDate, DateTime endDate)
        {
            try
            {

                var sales = _context.ProofOfPayments!
                    .Include(x => x.Customer!)
                    .Where(x =>
                        x.CreationTime >= startDate
                        && x.CreationTime <= endDate)
                    .ToList();

                if (sales.Any())
                {
                    var _sales = _mapper.Map<List<ProofOfPayment>, List<ProofOfPaymentDto>>(sales);

                    foreach (var item in _sales)
                    {
                        item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.UsedAmount = item.PaidAmount - item.UsableAmount;
                    }

                    var groupedSales = _sales
                    .GroupBy(x => x.CustomerId)
                    .Select(x => new ProofOfPaymentReportDto
                    {
                        CustomerId = x.Key,
                        ProofOfPayments = x.OrderByDescending(z => z.CreationTime).ToList(),
                        PaidTotal = x.ToList().Sum(z => z.PaidAmount),
                        CustomerName = _context.Companies!.FirstOrDefault(u => u.Id == x.Key)!.Name,
                        UsableTotal = x.ToList().Sum(z => z.UsableAmount),
                        UsedTotal = x.ToList().Sum(z => z.PaidAmount) - x.ToList().Sum(z => z.UsableAmount),
                    });

                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        Data = groupedSales,
                        IsSuccess = true,
                        Message = $"",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _logger.LogError("no sales where found for this day");
                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        IsSuccess = false,
                        Message = $"no sales where found for this day",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProofOfPaymentDto> Update(ProofOfPaymentDto proofOfPayment)
        {
            try
            {
                var _proofOfPayment = _mapper.Map<ProofOfPaymentDto, ProofOfPayment>(proofOfPayment);
                var res = _context.ProofOfPayments.Update(_proofOfPayment);
                _context.SaveChanges();

                return new ServiceResponse<ProofOfPaymentDto>
                {
                    Data = _mapper.Map<ProofOfPayment, ProofOfPaymentDto>(res.Entity),
                    Message = "ProofOfPayment Updated Successfully",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetMonthReportByCustomer(int month)
        {
            try
            {

                var sales = _context.ProofOfPayments!
                .Include(x => x.Customer!)
                    .Where(x => x.CreationTime.Month == month)
                    .ToList();

                if (sales.Any())
                {
                    var _sales = _mapper.Map<List<ProofOfPayment>, List<ProofOfPaymentDto>>(sales);

                    foreach (var item in _sales)
                    {
                        item.CreatorName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.UsedAmount = item.PaidAmount - item.UsableAmount;
                    }

                    var groupedSales = _sales
                    .GroupBy(x => x.CustomerId)
                    .Select(x => new ProofOfPaymentReportDto
                    {
                        CustomerId = x.Key,
                        CustomerName = _context.Companies!.FirstOrDefault(u => u.Id == x.Key)!.Name,
                        ProofOfPayments = x.OrderByDescending(z => z.CreationTime).ToList(),
                        PaidTotal = x.ToList().Sum(z => z.PaidAmount),
                        UsableTotal = x.ToList().Sum(z => z.UsableAmount),
                        UsedTotal = x.ToList().Sum(z => z.PaidAmount) - x.ToList().Sum(z => z.UsableAmount),
                    });

                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        Data = groupedSales,
                        IsSuccess = true,
                        Message = $"",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _logger.LogError("no sales where found for this day");
                    return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                    {
                        IsSuccess = false,
                        Message = $"no sales where found for this day",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<ProofOfPaymentReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
