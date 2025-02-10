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

namespace ProjectPos.Services.AppServices
{
    public class PaymentService : IPaymentService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<PaymentDto> Create(PaymentDto payment)
        {
            try
            {
                var pay = _mapper.Map<PaymentDto, Payment>(payment);

                var _payment = _context.Payments!.Add(pay);
                _context.SaveChanges();
                return new ServiceResponse<PaymentDto>
                {
                    Data = _mapper.Map<Payment, PaymentDto>(_payment.Entity),
                    IsSuccess = true,
                    Message = "payment Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating payment");
                return new ServiceResponse<PaymentDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PaymentDto> Delete(int id)
        {
            try
            {
                var payment = _context.Payments!.FirstOrDefault(x => x.Id == id);

                if (payment == null)
                {
                    _logger.LogError($"Payment with id: {id} does not exist");
                    return new ServiceResponse<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = "Payment Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    payment.IsDeleted = true;
                    _context.Payments!.Update(payment);
                    _context.SaveChanges();
                    return new ServiceResponse<PaymentDto>
                    {
                        IsSuccess = true,
                        Message = $"payment {payment.Id} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting payment");
                return new ServiceResponse<PaymentDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<PaymentDto>> GetAll()
        {
            try
            {
                var companies = _context.Payments!
                    .ToList();
                var _companies = _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(companies);
                return new ServiceResponse<IEnumerable<PaymentDto>>
                {
                    Data = _companies,
                    IsSuccess = true,
                    Message = $"Found  Companies",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<PaymentDto>> GetByOrder(int id)
        {
            try
            {
                var payment = _context.Payments!
                    .Where(x => x.SalesOrderId == id)
                    .ToList();

                if (payment == null)
                {
                    _logger.LogError($"Payment with id: {id} do not exist");
                    return new ServiceResponse<IEnumerable<PaymentDto>>
                    {
                        IsSuccess = false,
                        Message = $"Sales payments with Customer Id {id} Where Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _payment = _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDto>>(payment);
                    return new ServiceResponse<IEnumerable<PaymentDto>>
                    {
                        Data = _payment,
                        IsSuccess = true,
                        Message = $"Sales payments",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Sales payments by customer id");
                return new ServiceResponse<IEnumerable<PaymentDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PaymentDto> GetById(int id)
        {
            try
            {
                var payment = _context.Payments!
                    .FirstOrDefault(c => c.Id == id);

                if (payment == null)
                {
                    _logger.LogError($"Payment with id: {id} does not exist");
                    return new ServiceResponse<PaymentDto>
                    {
                        IsSuccess = false,
                        Message = $"payment {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _payment = _mapper.Map<Payment, PaymentDto>(payment);
                    return new ServiceResponse<PaymentDto>
                    {
                        Data = _payment,
                        IsSuccess = true,
                        Message = $"payment {payment.Id} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting payment by id");
                return new ServiceResponse<PaymentDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PaymentDto> Update(PaymentDto payment)
        {
            try
            {
                var pay = _mapper.Map<PaymentDto, Payment>(payment);

                var order = _context.SalesOrders!.FirstOrDefault(x => x.Id == pay.SalesOrderId && x.IsPaid == false);

                if (order == null)
                {
                    return new ServiceResponse<PaymentDto>
                    {
                        IsSuccess = true,
                        Message = $"Order Number {pay.SalesOrderId} Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    order.Balance -= pay.Amount;
                    _context.SalesOrders!.Update(order);

                    if (order.Balance == 0 && order.Balance < 0)
                    {
                        order.IsPaid = true;
                    }

                    var _payment = _context.Payments!.Update(pay);
                    _context.SaveChanges();
                    return new ServiceResponse<PaymentDto>
                    {
                        Data = _mapper.Map<Payment, PaymentDto>(_payment.Entity),
                        IsSuccess = true,
                        Message = "payment Updated Successfully",
                        Time = DateTime.Now,
                    };
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating payment");
                return new ServiceResponse<PaymentDto>
                {
                    IsSuccess = false,
                    Message = $"payment Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
