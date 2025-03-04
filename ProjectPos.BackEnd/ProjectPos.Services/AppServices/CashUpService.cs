using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectPos.Services.AppServices
{
    public class CashUpService : ICashUpService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CashUpService> _logger;

        public CashUpService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<CashUpService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public ServiceResponse<CashUpDto> Create(List<CashUpDto> cashUpDtos)
        {
            try
            {
                var cashUps = _mapper.Map<List<CashUpDto>, List<CashUp>>(cashUpDtos);
                _context.CashUps!.AddRange(cashUps);
                _context.SaveChanges();

                return new ServiceResponse<CashUpDto>
                {
                    IsSuccess = true,
                    Message = "Cashup Saved Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating cashUp");
                return new ServiceResponse<CashUpDto>
                {
                    IsSuccess = false,
                    Message = $"Cashup addition Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<CashUpDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<IEnumerable<GetCashUpList>> GetAll(DateTime date)
        {
            try
            {
                var day = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if (day == null)
                {
                    var cashUps = _context.CashUps!
                    .Where(x => x.CreationTime.Date == date.Date)
                    .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);

                        var groupedCashUps = _cashUps
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetCashUpList
                        {
                            UserId = x.Key,
                            CashUps = x.ToList(),
                            CashUpDate = x.Max(z => z.CreationTime),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName
                        });

                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Is Still Open",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    var cashUps = _context.CashUps!
                   .Where(x => x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay)
                   .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);

                        var groupedCashUps = _cashUps
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetCashUpList
                        {
                            UserId = x.Key,
                            CashUps = x.ToList(),
                            CashUpDate = x.Max(z => z.CreationTime),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName
                        });

                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day WAs Closed On {day.CreationTime}",
                            Time = DateTime.Now,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps by userid");
                return new ServiceResponse<IEnumerable<GetCashUpList>>
                {
                    IsSuccess = false,
                    Message = $"Getting cashup Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<CashUpReconDto>> GetUserCashUpByDate(DateTime date, int userId)
        {
            try
            {
                var user = await _context.SystemUsers!.FirstOrDefaultAsync(u => u.Id == userId);
                var payments = await _context.Payments
                    .Where(x => x.CreationTime.Date == date.Date && x.CreatorId == userId)
                    .ToListAsync();

                if (payments.Count == 0)
                {
                    return new ServiceResponse<CashUpReconDto>
                    {
                        IsSuccess = false,
                        Message = "No payments found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    
                    var cashUpRecon = new CashUpReconDto();
                    cashUpRecon.Name = user!.FullName;
                    cashUpRecon.PaymentTotal = payments.Sum(p => p.USDPaidAmount);
                    cashUpRecon.CashUpTotal = _context.CashUps!
                        .Where(x => x.CreatorId == userId && x.CreationTime.Date == date.Date)
                        .ToList()
                        .Sum(x => x.USDAmount);
                    cashUpRecon.CashReport = new List<CashReportDto>();
                    cashUpRecon.CreditCardReport = new List<CreditCardReportDto>();

                    foreach (string type in Enum.GetNames(typeof(SaleType)))
                    {
                        foreach (string currency in Enum.GetNames(typeof(Currency)))
                        {
                            if (type == SaleType.Cash.ToString())
                            {
                                var curr = payments.FirstOrDefault(payment => payment.Currency.ToString() == currency
                                        && payment.MethodOfPay.ToString() == type);
                                if (curr != null)
                                {
                                    var cashReport = new CashReportDto();
                                    cashReport.Currency = currency;
                                    cashReport.InHand = _context.CashUps!
                                        .Where(
                                            x => x.CreatorId == userId
                                            && x.CreationTime.Date == date.Date
                                            && x.Currency.ToString() == currency
                                            )
                                        .ToList()
                                        .Sum(x => x.Amount); 
                                    cashReport.UsdAmount = payments!
                                        .Where(
                                            x => x.MethodOfPay.ToString() == type
                                            && x.Currency.ToString() == currency
                                            && x.MethodOfPay.ToString() == type
                                            )
                                        .ToList()
                                        .Sum(x => x.USDPaidAmountAfterChange);
                                    cashReport.Amount = payments!
                                        .Where(
                                            x => x.MethodOfPay.ToString() == type
                                            && x.Currency.ToString() == currency
                                            && x.MethodOfPay.ToString() == type
                                            )
                                        .ToList()
                                        .Sum(x => x.PaidAmountAfterChange);
                                    cashUpRecon.CashReport.Add(cashReport);
                                }
                            }
                            else if (type == SaleType.CreditCard.ToString())
                            {
                                var curr = payments.FirstOrDefault(payment => payment.Currency.ToString() == currency
                                        && payment.MethodOfPay.ToString() == type);

                                if (curr != null)
                                {
                                    var creditCardReport = new CreditCardReportDto();
                                    creditCardReport.Currency = currency;
                                    creditCardReport.Amount = payments!
                                        .Where(
                                            x => x.MethodOfPay.ToString() == type
                                            && x.Currency.ToString() == currency
                                            && x.PaidAmount.ToString() == type)
                                        .ToList()
                                        .Sum(x => x.PaidAmount);
                                    cashUpRecon.CreditCardReport.Add(creditCardReport);
                                }
                            }
                            else if (type == SaleType.Credit.ToString())
                            {
                                var curr = payments.FirstOrDefault(payment => payment.Currency.ToString() == currency
                                        && payment.MethodOfPay.ToString() == type);

                                if (curr != null)
                                {
                                    var creditReport = new CreditReportDto();
                                    creditReport.Currency = currency;
                                    creditReport.Amount = payments!
                                        .Where(
                                            x => x.MethodOfPay.ToString() == type
                                            && x.Currency.ToString() == currency
                                            && x.PaidAmount.ToString() == type)
                                        .ToList()
                                        .Sum(x => x.PaidAmount);
                                    cashUpRecon.CreditReport!.Add(creditReport);
                                }
                            }
                        }
                    }

                    cashUpRecon.CashTotal = cashUpRecon.CashReport.Sum(x => x.UsdAmount);
                    cashUpRecon.ChangeAmount = cashUpRecon.CashUpTotal - cashUpRecon.CashTotal;

                    return new ServiceResponse<CashUpReconDto>
                    {
                        Data = cashUpRecon,
                        IsSuccess = true,
                        Message = "Payments found",
                        Time = DateTime.Now,
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps");
                return new ServiceResponse<CashUpReconDto>
                {
                    IsSuccess = false,
                    Message = $"Error while geting cashUps: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GetReconListDto>> GetAllRecon(DateTime date)
        {
            try
            {
                var day = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if (day == null)
                {
                    var cashUps = _context.CashUps!
                    .Where(x => x.CreationTime.Date == date.Date)
                    .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var recons = new List<ReconDto>();
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);
                        foreach (var cash in _cashUps)
                        {
                            var recon = new ReconDto
                            {
                                UserId = cash.CreatorId,
                                Amount = cash.Amount,
                                Currency = cash.Currency,
                                Rate = cash.Rate,
                                UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == cash.CreatorId)!.FullName,
                                USDAmount = cash.USDAmount,
                                SalesAmount = _context.Payments
                                            .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                            .ToList()
                                            .Sum(x => x.PaidAmount),
                                Variance = _context.Payments
                                        .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                        .ToList()
                                        .Sum(x => x.PaidAmount) - cash.USDAmount,
                                CashUpDate = cash.CreationTime
                            };

                            recons.Add(recon);
                        };

                        var groupedCashUps = recons
                        .GroupBy(x => x.UserId)
                        .Select(x => new GetReconListDto
                        {
                            UserId = x.Key,
                            Recons = x.ToList(),
                            CashUpDate = x.Max(z => z.CashUpDate),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            CashTotal = x.Sum(x => x.USDAmount),
                            SaleTotal = x.Sum(x => x.SalesAmount),
                        });

                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Is Still Open",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    var cashUps = _context.CashUps!
                    .Where(x => x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay)
                    .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var recons = new List<ReconDto>();
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);
                        foreach (var cash in _cashUps)
                        {
                            var recon = new ReconDto
                            {
                                UserId = cash.CreatorId,
                                Amount = cash.Amount,
                                Currency = cash.Currency,
                                Rate = cash.Rate,
                                UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == cash.CreatorId)!.FullName,
                                USDAmount = cash.USDAmount,
                                SalesAmount = _context.Payments
                                            .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                            .ToList()
                                            .Sum(x => x.PaidAmount),
                                Variance = _context.Payments
                                        .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                        .ToList()
                                        .Sum(x => x.PaidAmount) - cash.USDAmount,
                                CashUpDate = cash.CreationTime
                            };

                            recons.Add(recon);
                        };

                        var groupedCashUps = recons
                        .GroupBy(x => x.UserId)
                        .Select(x => new GetReconListDto
                        {
                            UserId = x.Key,
                            Recons = x.ToList(),
                            CashUpDate = x.Max(z => z.CashUpDate),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            CashTotal = x.Sum(x => x.USDAmount),
                            SaleTotal = x.Sum(x => x.SalesAmount),
                        });

                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Was Closed On {day.CreationTime}",
                            Time = DateTime.Now,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps by userid");
                return new ServiceResponse<IEnumerable<GetReconListDto>>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GetCashUpList>> GetByUserId(int id, DateTime date)
        {
            try
            {
                var day = _context.InventorySnapShotLogs!
                   .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if(day == null)
                {
                    var cashUps = _context.CashUps!
                        .Where(x => x.CreatorId == id && x.CreationTime.Date == date.Date)
                        .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found for userid {id}");
                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found for userid: {id}",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);

                        var groupedCashUps = _cashUps
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetCashUpList
                        {
                            UserId = x.Key,
                            CashUps = x.ToList(),
                            CashUpDate = x.Max(z => z.CreationTime),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName
                        });

                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Is Still Open",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    var cashUps = _context.CashUps!
                        .Where(x => x.CreatorId == id && x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay)
                        .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found for userid {id}");
                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found for userid: {id}",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);

                        var groupedCashUps = _cashUps
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetCashUpList
                        {
                            UserId = x.Key,
                            CashUps = x.ToList(),
                            CashUpDate = x.Max(z => z.CreationTime),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName
                        });

                        return new ServiceResponse<IEnumerable<GetCashUpList>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Was Closed On {day.CreationTime}",
                            Time = DateTime.Now,
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps by userid");
                return new ServiceResponse<IEnumerable<GetCashUpList>>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GetReconListDto>> GetReconByUserId(int userId, DateTime date)
        {
            try
            {

                var day = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if(day == null)
                {
                    var cashUps = _context.CashUps!
                        .Where(x => x.CreationTime.Date == date.Date && x.CreatorId == userId)
                        .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var recons = new List<ReconDto>();
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);
                        foreach (var cash in _cashUps)
                        {
                            var recon = new ReconDto
                            {
                                UserId = cash.CreatorId,
                                Amount = cash.Amount,
                                Currency = cash.Currency,
                                Rate = cash.Rate,
                                UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == cash.CreatorId)!.FullName,
                                USDAmount = cash.USDAmount,
                                SalesAmount = _context.Payments
                                            .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                            .ToList()
                                            .Sum(x => x.PaidAmount),
                                Variance = _context.Payments
                                        .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                        .ToList()
                                        .Sum(x => x.PaidAmount) - cash.USDAmount,
                                CashUpDate = cash.CreationTime
                            };

                            recons.Add(recon);
                        };

                        var groupedCashUps = recons
                        .GroupBy(x => x.UserId)
                        .Select(x => new GetReconListDto
                        {
                            UserId = x.Key,
                            Recons = x.ToList(),
                            CashUpDate = x.Max(z => z.CashUpDate),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            CashTotal = x.Sum(x => x.USDAmount),
                            SaleTotal = x.Sum(x => x.SalesAmount),
                        });

                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Is Still Open",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    var cashUps = _context.CashUps!
                        .Where(x => x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay && x.CreatorId == userId)
                        .ToList();

                    if (cashUps == null)
                    {
                        _logger.LogError($"No cashUps found");
                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            IsSuccess = false,
                            Message = $"No cashUps found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var recons = new List<ReconDto>();
                        var _cashUps = _mapper.Map<List<CashUp>, List<CashUpDto>>(cashUps);
                        foreach (var cash in _cashUps)
                        {
                            var recon = new ReconDto
                            {
                                UserId = cash.CreatorId,
                                Amount = cash.Amount,
                                Currency = cash.Currency,
                                Rate = cash.Rate,
                                UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == cash.CreatorId)!.FullName,
                                USDAmount = cash.USDAmount,
                                SalesAmount = _context.Payments
                                            .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                            .ToList()
                                            .Sum(x => x.PaidAmount),
                                Variance = _context.Payments
                                        .Where(x => x.OrderDate.Day == date.Day && x.Currency == cash.Currency && x.CreatorId == cash.CreatorId)
                                        .ToList()
                                        .Sum(x => x.PaidAmount) - cash.USDAmount,
                                CashUpDate = cash.CreationTime
                            };

                            recons.Add(recon);
                        };

                        var groupedCashUps = recons
                        .GroupBy(x => x.UserId)
                        .Select(x => new GetReconListDto
                        {
                            UserId = x.Key,
                            Recons = x.ToList(),
                            CashUpDate = x.Max(z => z.CashUpDate),
                            CashUpName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            CashTotal = x.Sum(x => x.USDAmount),
                            SaleTotal = x.Sum(x => x.SalesAmount),
                        });

                        return new ServiceResponse<IEnumerable<GetReconListDto>>
                        {
                            Data = groupedCashUps,
                            IsSuccess = true,
                            Message = $"This Day Was Closed On {day.CreationTime}",
                            Time = DateTime.Now,
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps by userid");
                return new ServiceResponse<IEnumerable<GetReconListDto>>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<ICollection<CashUpReconDto>>> GetAllCashUpsByDate(DateTime date)
        {
            try
            {
                var users = await _context.SystemUsers.ToListAsync();
                var payments = await _context.Payments
                    .Where(x => x.CreationTime.Date == date.Date)
                    .ToListAsync();
                if(payments.Count == 0)
                {
                    return new ServiceResponse<ICollection<CashUpReconDto>>
                    {
                        IsSuccess = false,
                        Message = "No payments found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var groupedPayments = payments.GroupBy(p => p.CreatorId)
                        .Select(g => new GroupedPaymentsDto
                        {
                            UserId = g.Key,
                            ChangeAmount = g.Sum(p => p.ChangeAmount),
                            InvoiceValue = g.Sum(p => p.TotalPrice),
                            TotalAmount = g.Sum(p => p.USDPaidAmountAfterChange),
                            Payments = g.Select(p => _mapper.Map<PaymentDto>(p)).ToList()
                        });

                    if (payments.Count == 0)
                    {
                        return new ServiceResponse<ICollection<CashUpReconDto>>
                        {
                            IsSuccess = false,
                            Message = "No payments found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var cashUpRecons = new List<CashUpReconDto>();
                        foreach (var payment in groupedPayments)
                        {
                            var cashUpRecon = new CashUpReconDto();
                            cashUpRecon.Name = users.FirstOrDefault(u => u.Id == payment.UserId)!.FullName;
                            cashUpRecon.PaymentTotal = payment.TotalAmount;
                            cashUpRecon.CashUpTotal = _context.CashUps!
                                .Where(x => x.CreatorId == payment.UserId && x.CreationTime.Date == date.Date)
                                .ToList()
                                .Sum(x => x.USDAmount);
                            cashUpRecon.CashReport = new List<CashReportDto>();
                            cashUpRecon.CreditCardReport = new List<CreditCardReportDto>();
                            cashUpRecon.EcoCashReport = new List<EcoCashReportDto>();

                            foreach (string type in Enum.GetNames(typeof(SaleType)))
                            {
                                foreach (string currency in Enum.GetNames(typeof(Currency)))
                                {
                                    if(payment.Payments!.FirstOrDefault(p => p.Currency.ToString() == currency && p.MethodOfPay.ToString() == type) != null)
                                    {
                                        if (type == SaleType.Cash.ToString())
                                        {
                                            var cashReport = new CashReportDto();
                                            cashReport.Currency = currency;
                                            cashReport.UsdAmount = payment.Payments!
                                                .Where(
                                                    x => x.MethodOfPay.ToString() == type
                                                    && x.Currency.ToString() == currency
                                                    && x.MethodOfPay.ToString() == type
                                                    )
                                                .ToList()
                                                .Sum(x => x.USDPaidAmountAfterChange);
                                            cashReport.Amount = payment.Payments!
                                                .Where(
                                                    x => x.MethodOfPay.ToString() == type
                                                    && x.Currency.ToString() == currency
                                                    )
                                                .ToList()
                                                .Sum(x => x.PaidAmountAfterChange);
                                            cashUpRecon.CashReport.Add(cashReport);
                                        }
                                        else if (type == SaleType.CreditCard.ToString())
                                        {
                                            var creditCardReport = new CreditCardReportDto();
                                            creditCardReport.Currency = currency;
                                            creditCardReport.Amount = payment.Payments!
                                                .Where(
                                                    x => x.MethodOfPay.ToString() == type
                                                    && x.Currency.ToString() == currency)
                                                .ToList()
                                                .Sum(x => x.PaidAmountAfterChange);
                                            cashUpRecon.CreditCardReport.Add(creditCardReport);
                                        }
                                        else if (type == SaleType.EcoCash.ToString())
                                        {
                                            var ecoCashReport = new EcoCashReportDto();
                                            ecoCashReport.Currency = currency;
                                            ecoCashReport.Amount = payment.Payments!
                                                .Where(
                                                    x => x.MethodOfPay.ToString() == type
                                                    && x.Currency.ToString() == currency)
                                                .ToList()
                                                .Sum(x => x.PaidAmountAfterChange);
                                            cashUpRecon.EcoCashReport.Add(ecoCashReport);
                                        }
                                    }
                                }
                            }


                            cashUpRecon.CashTotal = cashUpRecon.CashReport.Sum(x => x.UsdAmount);
                            cashUpRecon.ChangeAmount = cashUpRecon.CashUpTotal - cashUpRecon.CashTotal;

                            cashUpRecons.Add(cashUpRecon);
                        }

                        return new ServiceResponse<ICollection<CashUpReconDto>>
                        {
                            Data = cashUpRecons,
                            IsSuccess = true,
                            Message = "Payments found",
                            Time = DateTime.Now,
                        };
                    }

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting cashUps");
                return new ServiceResponse<ICollection<CashUpReconDto>>
                {
                    IsSuccess = false,
                    Message = $"Error while geting cashUps: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<CashUpDto> Update(CashUpDto cashUp)
        {
            throw new NotImplementedException();
        }
    }
}
