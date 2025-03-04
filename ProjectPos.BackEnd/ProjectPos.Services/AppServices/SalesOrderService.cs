using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using ProjectPos.Services.ReportingDtos;

namespace ProjectPos.Services.AppServices
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesOrderService> _logger;

        public SalesOrderService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<SalesOrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public ServiceResponse<SalesOrderDto> Create(SalesOrderDto order)
        {
            try
            {
                var ord = _mapper.Map<SalesOrderDto, SalesOrder>(order);
                var _order = _context.SalesOrders!.Add(ord);

                foreach (var item in order.SalesOrderItems!)
                {
                    var prod = _context.ProductInventories!.FirstOrDefault(x => x.Id == item.ProductId);
                    prod.QuantityOnHand -= item.Quantity;

                    if(prod.QuantityOnHand < prod.IdealQuantity && prod.QuantityOnHand > 0)
                    {
                        prod.Status = Status.LowStock;
                    }
                    else if (prod.QuantityOnHand < 1)
                    {
                        prod.Status = Status.OutOfStock;
                    }

                    _context.ProductInventories!.Update(prod);
                }

               
                _context.SaveChanges();
                return new ServiceResponse<SalesOrderDto>
                {
                    Data = _mapper.Map<SalesOrder, SalesOrderDto>(_order.Entity),
                    IsSuccess = true,
                    Message = "Order Saved Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return new ServiceResponse<SalesOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Order Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SalesOrderDto> Delete(int id)
        {
            try
            {
                var order = _context.SalesOrders!.FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    _logger.LogError($"SalesOrder with id: {id} does not exist");
                    return new ServiceResponse<SalesOrderDto>
                    {
                        IsSuccess = false,
                        Message = "SalesOrder Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    order.IsDeleted = true;
                    _context.SalesOrders!.Update(order);
                    _context.SaveChanges();
                    return new ServiceResponse<SalesOrderDto>
                    {
                        IsSuccess = true,
                        Message = $"order {order.Id} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order");
                return new ServiceResponse<SalesOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll()
        {
            try
            {
                var companies = _context.SalesOrders!
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .ToList();
                var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                foreach (var item in _companies)
                {
                    item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                }

                return new ServiceResponse<IEnumerable<SalesOrderDto>>
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
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
        public async Task<ServiceResponse<IEnumerable<SalesOrderDto>>> GetAllToday()
        {
            try
            {
                var date = DateTime.Today;
                var companies = await _context.SalesOrders!
                    .Include(x => x.Customer!)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date == date)
                    .ToListAsync();
                var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                foreach (var item in _companies)
                {
                    item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                }

                return new ServiceResponse<IEnumerable<SalesOrderDto>>
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
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems(int? id)
        {
            try
            {
                var products = _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .Where(x => x.SalesOrder!.CreationTime.Date == DateTime.Today)
                    .OrderBy(x => x.ProductName)
                    .ToList();

                var _products = _mapper.Map<IEnumerable<SalesOrderItem>, IEnumerable<SalesOrderItemDto>>(products);

                var grouped = products
                    .GroupBy(x => x.ProductName)
                    .Select(x => new GroupedSalesOrderItemDto
                    {
                        Category = x.FirstOrDefault().Product!.Category,
                        SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                        SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                        ProductName = x.Key,
                        Quantity = x.Sum(z => z.Quantity),
                        UnitPrice = x.FirstOrDefault().UnitPrice,
                        Price = x.Sum(z => z.Price),
                        BarCode = x.FirstOrDefault().BarCode,
                        ProductId = x.FirstOrDefault().ProductId,
                        Unit = x.FirstOrDefault().Unit
                    });

                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    Data = grouped,
                    IsSuccess = true,
                    Message = $"Found  products",
                    Time = DateTime.Now,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all products");
                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetMonthSalesOrderItems(int month, int? id)
{
    try
    {
        var products = new List<SalesOrderItem>();
        if(id == 0)
        {
            products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .Where(x => x.SalesOrder!.CreationTime.Month == month)
            .OrderBy(x => x.ProductName)
            .ToList();
        }
        else
        {
           products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .Where(x => x.SalesOrder!.CreationTime.Month == month && x.SalesOrder.CreatorId == id)
            .OrderBy(x => x.ProductName)
            .ToList();
        }

        var _products = _mapper.Map<IEnumerable<SalesOrderItem>, IEnumerable<SalesOrderItemDto>>(products);

        var grouped = products
            .GroupBy(x => x.ProductName)
            .Select(x => new GroupedSalesOrderItemDto
            {
                Category = x.FirstOrDefault().Product!.Category,
                SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                ProductName = x.Key,
                Quantity = x.Sum(z => z.Quantity),
                UnitPrice = x.FirstOrDefault().UnitPrice,
                Price = x.Sum(z => z.Price),
                BarCode = x.FirstOrDefault().BarCode,
                ProductId = x.FirstOrDefault().ProductId,
                Unit = x.FirstOrDefault().Unit
            });

        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            Data = grouped,
            IsSuccess = true,
            Message = $"Found  products",
            Time = DateTime.Now,
        };

    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error while getting all products");
        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            IsSuccess = false,
            Message = $"Network Failed: {ex.Message}",
            Time = DateTime.Now,
        };
    }
}

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems(DateTime date, int? id)
{
    try
    {
        var products = new List<SalesOrderItem>();

        if (id == 0)
        {
            products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .OrderBy(x => x.ProductName)
            .Where(x => x.SalesOrder!.CreationTime.Date == date.Date)
            .ToList();
        }
        else
        {
            products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .OrderBy(x => x.ProductName)
            .Where(x => x.SalesOrder!.CreationTime.Date == date.Date && x.SalesOrder.CreatorId == id)
            .ToList();
        }
        
        var categories = _context.SubCategories!.ToList();

        var grouped = products
            .GroupBy(x => x.ProductName)
            .Select(x => new GroupedSalesOrderItemDto
            {
                Category = x.FirstOrDefault()!.Product!.Category,
                SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                ProductName = x.Key,
                Quantity = x.Sum(z => z.Quantity),
                UnitPrice = x.FirstOrDefault()!.UnitPrice,
                Price = x.Sum(z => z.Price),
                BarCode = x.FirstOrDefault()!.BarCode,
                ProductId = x.FirstOrDefault()!.ProductId,
                Unit = x.FirstOrDefault()!.Unit
            });

        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            Data = grouped,
            IsSuccess = true,
            Message = $"Found  products",
            Time = DateTime.Now,
        };

    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error while getting all products");
        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            IsSuccess = false,
            Message = $"Network Failed: {ex.Message}",
            Time = DateTime.Now,
        };
    }
}

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItemsByRange(DateTime start, DateTime end, int? id)
{
    try
    {
        var products = new List<SalesOrderItem>();

        if (id == 0)
        {
            products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .OrderBy(x => x.ProductName)
            .Where(x => x.SalesOrder!.CreationTime.Date >= start.Date && x.SalesOrder!.CreationTime.Date <= end.Date)
            .ToList();
        }
        else
        {
            products = _context.SalesOrderItems!
            .Include(x => x.Product)
            .Include(x => x.SalesOrder)
            .OrderBy(x => x.ProductName)
            .Where(x => x.SalesOrder!.CreationTime.Date >= start.Date && x.SalesOrder!.CreationTime.Date <= end.Date && x.SalesOrder.CreatorId == id)
            .ToList();
        }

        var categories = _context.SubCategories!.ToList();

        var grouped = products
            .GroupBy(x => x.ProductName)
            .Select(x => new GroupedSalesOrderItemDto
            {
                Category = x.FirstOrDefault()!.Product!.Category,
                SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                ProductName = x.Key,
                Quantity = x.Sum(z => z.Quantity),
                UnitPrice = x.FirstOrDefault()!.UnitPrice,
                Price = x.Sum(z => z.Price),
                BarCode = x.FirstOrDefault()!.BarCode,
                ProductId = x.FirstOrDefault()!.ProductId,
                Unit = x.FirstOrDefault()!.Unit
            });

        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            Data = grouped,
            IsSuccess = true,
            Message = $"Found  products",
            Time = DateTime.Now,
        };

    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error while getting all products");
        return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
        {
            IsSuccess = false,
            Message = $"Network Failed: {ex.Message}",
            Time = DateTime.Now,
        };
    }
}

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll(DateTime date)
        {
            try
            {
                var day = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if (day == null)
                {
                    var companies = _context.SalesOrders!
                        .Include(x => x.Payments)
                        .Include(x => x.Customer!)
                            .ThenInclude(c => c.Address)
                        .Include(x => x.SalesOrderItems!)
                            .ThenInclude(x => x.Product)
                        .Where(x => x.CreationTime.Date == date.Date)
                        .ToList();

                    var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                    foreach (var item in _companies)
                    {
                        item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                    }

                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        Data = _companies,
                        IsSuccess = true,
                        Message = $"This Day Is Still Open",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var companies = _context.SalesOrders!
                        .Include(x => x.Customer!)
                            .ThenInclude(c => c.Address)
                        .Include(x => x.SalesOrderItems!)
                            .ThenInclude(x => x.Product)
                        .Where(x => x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay)
                        .ToList();

                    var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                    foreach (var item in _companies)
                    {
                        item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                    }

                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        Data = _companies,
                        IsSuccess = true,
                        Message = $"Found  Companies",
                        Time = DateTime.Now,
                    };

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll(DateTime startDate, DateTime endDate)
        {
            try
            {
                startDate = startDate.AddDays(1);
                endDate = endDate.AddDays(1);
                var companies = _context.SalesOrders!
                    .Include(x => x.Payments)
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                        .Where(x =>
                    x.CreationTime >= startDate
                    && x.CreationTime <= endDate
                    ).ToList();

                var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                foreach (var item in _companies)
                {
                    item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                }

                return new ServiceResponse<IEnumerable<SalesOrderDto>>
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
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GetSalesOrderByUserDto>> GetAllDate(DateTime date)
        {
            try
            {
                date = date.AddDays(1);

                var day = _context.InventorySnapShotLogs!
                            .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                if (day == null)
                {
                    var sales = _context.SalesOrders!
                        .Include(c => c.Payments)
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date == date.Date)
                    .ToList();

                    if (sales.Any())
                    {
                        var _sales = _mapper.Map<List<SalesOrder>, List<SalesOrderDto>>(sales);

                        foreach (var item in _sales)
                        {
                            item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                            item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                        }

                        var groupedSales = _sales
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetSalesOrderByUserDto
                        {
                            UserId = x.Key,
                            SalesOrders = x.OrderByDescending(z => z.CreationTime).ToList(),
                            AmountTotal = x.ToList().Sum(z => z.Price),
                            MethodOfPay = x.FirstOrDefault().Payments.FirstOrDefault(p => p.SalesOrderId == x.FirstOrDefault().Id).MethodOfPay,
                           EcocashSuccessCode = x.FirstOrDefault().Payments.FirstOrDefault(p => p.SalesOrderId == x.FirstOrDefault().Id).EcocashSuccessCode,
                            UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            BalanceTotal = x.ToList().Sum(z => z.Balance),
                            PaidTotal = x.ToList().Sum(z => z.Price) - x.ToList().Sum(z => z.Balance),
                        });

                        return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                        {
                            Data = groupedSales,
                            IsSuccess = true,
                            Message = $"This Day Is Still Open",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        _logger.LogError("no sales where found for this day");
                        return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                        {
                            IsSuccess = false,
                            Message = $"no sales where found for this day",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    var sales = _context.SalesOrders!
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime <= day.CreationTime && x.CreationTime >= day.StartDay)
                    .ToList();

                    if (sales.Any())
                    {
                        var _sales = _mapper.Map<List<SalesOrder>, List<SalesOrderDto>>(sales);

                        foreach (var item in _sales)
                        {
                            item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                            item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                        }

                        var groupedSales = _sales
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new GetSalesOrderByUserDto
                        {
                            UserId = x.Key,
                            SalesOrders = x.OrderByDescending(z => z.CreationTime).ToList(),
                            AmountTotal = x.ToList().Sum(z => z.Price),
                            UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                            BalanceTotal = x.ToList().Sum(z => z.Balance),
                            PaidTotal = x.ToList().Sum(z => z.Price) - x.ToList().Sum(z => z.Balance),
                        });

                        return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                        {
                            Data = groupedSales,
                            IsSuccess = true,
                            Message = $"This Day Was Closed On {day.CreationTime}",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        _logger.LogError("no sales where found for this day");
                        return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                        {
                            IsSuccess = false,
                            Message = $"no sales where found for this day",
                            Time = DateTime.Now,
                        };
                    }
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAllMonth(int month)
        {
            try
            {
                var companies = _context.SalesOrders!
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Month == month)
                    .ToList();

                var _companies = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(companies);

                foreach (var item in _companies)
                {
                    item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                    item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                }

                return new ServiceResponse<IEnumerable<SalesOrderDto>>
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
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GetSalesOrderByUserDto>> GetAllRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                startDate = startDate.AddDays(1);
                endDate = endDate.AddDays(1);
                var sales = _context.SalesOrders!
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => 
                    x.CreationTime > startDate.AddDays(-1) 
                    && x.CreationTime < endDate.AddDays(1)
                    ).ToList();

                if (sales.Any())
                {
                    var _sales = _mapper.Map<List<SalesOrder>, List<SalesOrderDto>>(sales);

                    foreach (var item in _sales)
                    {
                        item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                    }

                    var groupedSales = _sales
                    .GroupBy(x => x.CreatorId)
                    .Select(x => new GetSalesOrderByUserDto
                    {
                        UserId = x.Key,
                        SalesOrders = x.OrderByDescending(z => z.CreationTime).ToList(),
                        AmountTotal = x.ToList().Sum(z => z.Price),
                        MethodOfPay = x.FirstOrDefault().Payments.FirstOrDefault(p => p.SalesOrderId == x.FirstOrDefault().Id).MethodOfPay,
                        EcocashSuccessCode = x.FirstOrDefault().Payments.FirstOrDefault(p => p.SalesOrderId == x.FirstOrDefault().Id).EcocashSuccessCode,
                        UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                        BalanceTotal = x.ToList().Sum(z => z.Balance),
                        PaidTotal = x.ToList().Sum(z => z.Price) - x.ToList().Sum(z => z.Balance),
                    });

                    return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
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
                    return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
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
                return new ServiceResponse<IEnumerable<GetSalesOrderByUserDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetByCustomerId(int id)
        {
            try
            {
                var order = _context.SalesOrders!
                    .Where(x => x.CustomerId == id)
                    .ToList();

                if (order == null)
                {
                    _logger.LogError($"SalesOrder with id: {id} do not exist");
                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        IsSuccess = false,
                        Message = $"Sales Orders with Customer Id {id} Where Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _order = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(order);
                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        Data = _order,
                        IsSuccess = true,
                        Message = $"Sales Orders",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Sales Orders by customer id");
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SalesOrderDto> GetById(int id)
        {
            try
            {
                
                var order = _context.SalesOrders!
                    .Include(o => o.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product!)
                    .Include(x => x.Payments)
                    .FirstOrDefault(x => x.Id == id);

                //var order = orders.FirstOrDefault(x => x.Id == id);
                if (order.CreationTime < DateTime.Today)
                {
                    return new ServiceResponse<SalesOrderDto>
                    {
                        IsSuccess = false,
                        Message = $"Return period closed",
                        Time = DateTime.Now,
                    };
                }
                else if (order == null)
                {
                    _logger.LogError($"SalesOrder with id: {id} does not exist");
                    return new ServiceResponse<SalesOrderDto>
                    {
                        IsSuccess = false,
                        Message = $"order {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _order = _mapper.Map<SalesOrder, SalesOrderDto>(order);
                    return new ServiceResponse<SalesOrderDto>
                    {
                        Data = _order,
                        IsSuccess = true,
                        Message = $"order {order.Id} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting order by id");
                return new ServiceResponse<SalesOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetByName(string name)   
        {
            try
            {
                var order = _context.SalesOrders!
                    .Where(x => x.Customer!.Name == name)
                    .ToList();

                if (order == null)
                {
                    _logger.LogError($"SalesOrder for: {name} do not exist");
                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        IsSuccess = false,
                        Message = $"Sales Orders for Customer {name} Where Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _order = _mapper.Map<IEnumerable<SalesOrder>, IEnumerable<SalesOrderDto>>(order);
                    return new ServiceResponse<IEnumerable<SalesOrderDto>>
                    {
                        Data = _order,
                        IsSuccess = true,
                        Message = $"Sales Orders",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Sales Orders by customer name");
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesEndDayReportDto>> GetByUserId()
        {
            try
            {

                var sales = _context.SalesOrders!
                    .Include(x => x.Customer!)
                        .ThenInclude(c => c.Address)
                    .Include(x => x.SalesOrderItems!)
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date == DateTime.Today)
                    .ToList();

                if (sales.Any())
                {
                    var _sales = _mapper.Map<List<SalesOrder>, List<SalesOrderDto>>(sales);

                    foreach (var item in _sales)
                    {
                        item.UserName = _context.SystemUsers!.FirstOrDefault(x => x.Id == item.CreatorId)!.FullName;
                        item.PaidAmount = _context.Payments.Any(x => x.SalesOrderId == item.Id) ? _context.Payments.Where(x => x.SalesOrderId == item.Id).Sum(x => x.PaidAmount) : 0;
                    }

                    var groupedSales = _sales
                    .GroupBy(x => x.CreatorId)
                    .Select(x => new GetSalesOrderByUserDto
                    {
                        UserId = x.Key,
                        SalesOrders = x.OrderByDescending(z => z.CreationTime).ToList(),
                        AmountTotal = x.ToList().Sum(z => z.Price),
                        UserName = _context.SystemUsers!.FirstOrDefault(y => y.Id == x.Key)!.FullName,
                        BalanceTotal = x.ToList().Sum(z => z.Balance),
                        PaidTotal = x.ToList().Sum(z => z.Price) - x.ToList().Sum(z => z.Balance),
                    });

                    var report = groupedSales.Select(rep => new SalesEndDayReportDto
                    {
                        UserName = rep.UserName,
                        SalesTotal = rep.AmountTotal,
                        CashTotal = rep.PaidTotal,
                        Variance = rep.BalanceTotal,
                        Date = DateTime.Today
                    });

                    return new ServiceResponse<IEnumerable<SalesEndDayReportDto>>
                    {
                        Data = report,
                        IsSuccess = true,
                        Message = $"",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _logger.LogError("no sales where found for this day");
                    return new ServiceResponse<IEnumerable<SalesEndDayReportDto>>
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
                return new ServiceResponse<IEnumerable<SalesEndDayReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems()
        {
            try
            {
                var products = _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .Where(x => x.SalesOrder!.CreationTime.Date == DateTime.Today)
                    .OrderBy(x => x.ProductName)
                    .ToList();

                var _products = _mapper.Map<IEnumerable<SalesOrderItem>, IEnumerable<SalesOrderItemDto>>(products);

                var grouped = products
                    .GroupBy(x => x.ProductName)
                    .Select(x => new GroupedSalesOrderItemDto
                    {
                        Category = x.FirstOrDefault().Product!.Category,
                        SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                        SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                        ProductName = x.Key,
                        Quantity = x.Sum(z => z.Quantity),
                        UnitPrice = x.FirstOrDefault().UnitPrice,
                        Price = x.Sum(z => z.Price),
                        BarCode = x.FirstOrDefault().BarCode,
                        ProductId = x.FirstOrDefault().ProductId,
                        Unit = x.FirstOrDefault().Unit
                    });

                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    Data = grouped,
                    IsSuccess = true,
                    Message = $"Found  products",
                    Time = DateTime.Now,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all products");
                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetMonthSalesOrderItems(int month)
        {
            try
            {
                var products = _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .Where(x => x.SalesOrder!.CreationTime.Month == month)
                    .OrderBy(x => x.ProductName)
                    .ToList();

                var _products = _mapper.Map<IEnumerable<SalesOrderItem>, IEnumerable<SalesOrderItemDto>>(products);

                var grouped = products
                    .GroupBy(x => x.ProductName)
                    .Select(x => new GroupedSalesOrderItemDto
                    {
                        Category = x.FirstOrDefault().Product!.Category,
                        SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                        SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                        ProductName = x.Key,
                        Quantity = x.Sum(z => z.Quantity),
                        UnitPrice = x.FirstOrDefault().UnitPrice,
                        Price = x.Sum(z => z.Price),
                        BarCode = x.FirstOrDefault().BarCode,
                        ProductId = x.FirstOrDefault().ProductId,
                        Unit = x.FirstOrDefault().Unit
                    });

                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    Data = grouped,
                    IsSuccess = true,
                    Message = $"Found  products",
                    Time = DateTime.Now,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all products");
                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems(DateTime date, int id)
        {
            try
            {
                var products = new List<SalesOrderItem>();

                if (id == 0)
                {
                    products = _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .OrderBy(x => x.ProductName)
                    .Where(x => x.SalesOrder!.CreationTime.Date == date.Date)
                    .ToList();
                }
                else
                {
                    products = _context.SalesOrderItems!
                    .Include(x => x.Product)
                    .Include(x => x.SalesOrder)
                    .OrderBy(x => x.ProductName)
                    .Where(x => x.SalesOrder!.CreationTime.Date == date.Date && x.SalesOrder.CreatorId == id)
                    .ToList();
                }
                
                var categories = _context.SubCategories!.ToList();

                var grouped = products
                    .GroupBy(x => x.ProductName)
                    .Select(x => new GroupedSalesOrderItemDto
                    {
                        Category = x.FirstOrDefault()!.Product!.Category,
                        SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                        ProductName = x.Key,
                        Quantity = x.Sum(z => z.Quantity),
                        UnitPrice = x.FirstOrDefault()!.UnitPrice,
                        Price = x.Sum(z => z.Price),
                        BarCode = x.FirstOrDefault()!.BarCode,
                        ProductId = x.FirstOrDefault()!.ProductId,
                        Unit = x.FirstOrDefault()!.Unit
                    });

                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    Data = grouped,
                    IsSuccess = true,
                    Message = $"Found  products",
                    Time = DateTime.Now,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all products");
                return new ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<SalesOrderDto>> GetTodayByName()
        {
            try
            {
                var sales = _context.SalesOrders!
                    .Where(x => x.CreationTime.Date == DateTime.Today)
                    .ToList();

                

                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Order Update Failed",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return new ServiceResponse<IEnumerable<SalesOrderDto>>
                {
                    IsSuccess = false,
                    Message = $"Order Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SalesOrderDto> Update(SalesOrderDto order)
        {
            try
            {
                
                var ord = _mapper.Map<SalesOrderDto, SalesOrder>(order);

                ord.SalesOrderItems = AdjustStockOnUpdate(ord.SalesOrderItems);
                var _order = _context.SalesOrders!.Update(ord);
                _context.SaveChanges();
                return new ServiceResponse<SalesOrderDto>
                {
                    Data = _mapper.Map<SalesOrder, SalesOrderDto>(_order.Entity),
                    IsSuccess = true,
                    Message = "Order Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return new ServiceResponse<SalesOrderDto>
                {
                    IsSuccess = false,
                    Message = $"Order Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ICollection<SalesOrderItem> AdjustStockOnUpdate(ICollection<SalesOrderItem> orders)
        {
            
            
            foreach (var item in orders)
            {
                if (item.Quantity < 0)
                {
                    var product = _context.ProductInventories.AsNoTracking().FirstOrDefault(i => i.Id == item.ProductId);
                    var toBeReturned = 0 - item.Quantity;
                    product.QuantityOnHand += toBeReturned;
                        ;
                    _context.ProductInventories.Update(product);
                    _context.SaveChanges();
                    
                    item.isReturned = true;
                    
                }

                

            }

            return orders;



        }


        // Helper method to adjust account balance
        static decimal AdjustAccountBalance(AccountType accountType, JournalEntryType entryType, decimal amount)
        {
            return accountType switch
            {
                AccountType.Assets or AccountType.Expense => entryType == JournalEntryType.Debit ? amount : -amount,
                AccountType.Liability or AccountType.Equity or AccountType.Revenue or AccountType.Income => entryType == JournalEntryType.Credit ? amount : -amount,
                _ => throw new Exception($"Unhandled account type: {accountType}")
            };
        }
    }
}
