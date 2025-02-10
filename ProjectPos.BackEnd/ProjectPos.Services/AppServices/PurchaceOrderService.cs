using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices
{
    public class PurchaceOrderService : IPurchaceOrderService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaceOrderService> _logger;

        public PurchaceOrderService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<PurchaceOrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<PurchaceOrderDto> Create(PurchaceOrderDto orderDto)
        {
            try
            {
                var order = _mapper.Map<PurchaceOrderDto, PurchaceOrder>(orderDto);
                var _order = _context.PurchaceOrders.Add(order);
                _context.SaveChanges();

                var ord = _context.PurchaceOrders
                    .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .FirstOrDefault(x => x.Id == _order.Entity.Id);
                return new ServiceResponse<PurchaceOrderDto>
                {
                    Data = _mapper.Map<PurchaceOrder, PurchaceOrderDto>(ord),
                    IsSuccess = true,
                    Message = "Customer Registered Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return new ServiceResponse<PurchaceOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceOrderDto> Delete(int id)
        {
            try
            {
                var order = _context.PurchaceOrders!.FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    _logger.LogError($"PurchaceOrder with id: {id} does not exist");
                    return new ServiceResponse<PurchaceOrderDto>
                    {
                        IsSuccess = false,
                        Message = "PurchaceOrder Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.PurchaceOrders!.Remove(order!);
                    _context.SaveChanges();
                    return new ServiceResponse<PurchaceOrderDto>
                    {
                        IsSuccess = true,
                        Message = $"order {order.Company!.Name} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order");
                return new ServiceResponse<PurchaceOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<PurchaceOrderDto>> GetAll()
        {
            try
            {
                var orders = _context.PurchaceOrders
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .ToList();
                var _orders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(orders);
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _orders,
                    IsSuccess = true,
                    Message = $"Found {_orders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all orders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceOrderDto> GetById(int id)
        {
            try
            {
                var order = _context.PurchaceOrders
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .FirstOrDefault(c => c.Id == id);

                if (order == null)
                {
                    _logger.LogError($"PurchaceOrder with id: {id} does not exist");
                    return new ServiceResponse<PurchaceOrderDto>
                    {
                        IsSuccess = false,
                        Message = $"order {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _order = _mapper.Map<PurchaceOrder, PurchaceOrderDto>(order);
                    return new ServiceResponse<PurchaceOrderDto>
                    {
                        Data = _order,
                        IsSuccess = true,
                        Message = $"order {order.Company!.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting order by id");
                return new ServiceResponse<PurchaceOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<PurchaceOrderDto>> GetByName(string name)
        {
            try
            {
                var orders = _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(c => c.Company!.Name == name)
                    .ToList();

                if (orders == null)
                {
                    _logger.LogError($"PurchaceOrder with name: {name} does not exist");
                    return new ServiceResponse<List<PurchaceOrderDto>>
                    {
                        IsSuccess = false,
                        Message = $"PurchaceOrders with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _orders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(orders);

                    return new ServiceResponse<List<PurchaceOrderDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_orders.Count} PurchaceOrders",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting order by name");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceOrderDto> Update(PurchaceOrderDto orderDto)
        {
            try
            {
                var order = _mapper.Map<PurchaceOrderDto, PurchaceOrder>(orderDto);
                var _order = _context.PurchaceOrders!.Update(order);
                _context.SaveChanges();
                return new ServiceResponse<PurchaceOrderDto>
                {
                    Data = _mapper.Map<PurchaceOrder, PurchaceOrderDto>(_order.Entity),
                    IsSuccess = true,
                    Message = "Customer UpDated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating order");
                return new ServiceResponse<PurchaceOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }


        public async Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllToday()
        {
            try
            {
                var purchaceOrders = await _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(x => x.CreationTime.Date == DateTime.Today && x.IsDeleted == false)
                    .ToListAsync();

                var _purchaceOrders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(purchaceOrders);

                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _purchaceOrders,
                    IsSuccess = true,
                    Message = $"Found {_purchaceOrders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceOrders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByDate(DateTime date)
        {
            try
            {
                var purchaceOrders = await _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(x => x.CreationTime.Date == date.Date && x.IsDeleted == false)
                    .ToListAsync();

                var _purchaceOrders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(purchaceOrders);

                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _purchaceOrders,
                    IsSuccess = true,
                    Message = $"Found {_purchaceOrders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceOrders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByDateRange(DateTime start, DateTime end)
        {
            try
            {
                var purchaceOrders = await _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(x => x.CreationTime.Date > start.Date && x.CreationTime.Date < end.Date.AddDays(1) && x.IsDeleted == false)
                    .ToListAsync();

                var _purchaceOrders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(purchaceOrders);

                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _purchaceOrders,
                    IsSuccess = true,
                    Message = $"Found {_purchaceOrders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceOrders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllBySupplier(int supplierId)
        {
            try
            {
                var purchaceOrders = await _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(x => x.SupplierId == supplierId && x.IsDeleted == false)
                    .ToListAsync();

                var _purchaceOrders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(purchaceOrders);

                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _purchaceOrders,
                    IsSuccess = true,
                    Message = $"Found {_purchaceOrders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceOrders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByMonth(int month)
        {
            try
            {
                var year = DateTime.Now.Year;
                var purchaceOrders = await _context.PurchaceOrders!
                     .Include(x => x.Company)
                    .Include(x => x.PurchaceOrderItems)!
                        .ThenInclude(z => z.Product)
                    .Where(x => x.CreationTime.Month == month && x.CreationTime.Year == year && x.IsDeleted == false)
                    .ToListAsync();

                var _purchaceOrders = _mapper.Map<List<PurchaceOrder>, List<PurchaceOrderDto>>(purchaceOrders);

                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    Data = _purchaceOrders,
                    IsSuccess = true,
                    Message = $"Found {_purchaceOrders.Count} PurchaceOrders",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceOrders");
                return new ServiceResponse<List<PurchaceOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
