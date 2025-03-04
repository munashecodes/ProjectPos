using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class GoodsReceivedVoucherLineService : IGoodsReceivedVoucherLineService
{
    private readonly ProjectPosDbContext _context;
    private readonly ILogger<GoodsReceivedVoucherLineService> _logger;
    private readonly IMapper _mapper;

    public GoodsReceivedVoucherLineService(
        ProjectPosDbContext context, 
        ILogger<GoodsReceivedVoucherLineService> logger, 
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public ServiceResponse<List<GroupedGrvItemsDto>> GetByDateGrvItems(DateTime date)
    {
        try
        {
            var openingInventorySnap = _context.ProductInventorySnapshots!
                .Where(x => x.CreationTime.Date <= date.Date.AddDays(-1) && x.SnapShotType == SnapShotEnum.CloseDay)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefault();

            var openingInventory = new List<ProductInventory>();

            if (openingInventorySnap != null)
            {
                openingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
            }

            var closingInventory = new List<ProductInventory>();

            if (date > DateTime.Today)
            {
                return new ServiceResponse<List<GroupedGrvItemsDto>>
                {
                    IsSuccess = false,
                    Message = $"Dates Out Of Range",
                    Time = DateTime.Now,
                };
            }

            if (date.Date == DateTime.Today)
            {
                closingInventory = _context.ProductInventories!
                    .ToList();
            }
            else
            {
                var closingInventorySnap = _context.ProductInventorySnapshots!
                                            .Where(x => x.CreationTime.Date >= date.Date)
                                            .OrderByDescending(x => x.CreationTime)
                                            .LastOrDefault();

                if (closingInventorySnap != null)
                {
                    closingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
                }
                else
                {
                    closingInventory = _context.ProductInventories!
                    .ToList();
                }
            }

            var products = _context.GoodsReceivedVoucherLines!
                 .Include(x => x.Product)
                 .Include(x => x.GoodsReceivedVoucher)
                 .Where(x => x.GoodsReceivedVoucher!.CreationTime.Date == date.Date && x.GoodsReceivedVoucher.IsApproved)
                 .OrderBy(x => x.Product!.Name)
                 .ToList();
            
            var productPrices = _context.ProductPrices!.ToList();

            var _products = _mapper.Map<IEnumerable<GoodsReceivedVoucherLine>, IEnumerable<GoodsReceivedVoucherLineDto>>(products);

            var grouped = products
                .GroupBy(x => x.Product!.Name)
                .Select(x => new GroupedGrvItemsDto
                {
                    Category = x.FirstOrDefault().Product!.Category,
                    SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                    SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                    ProductName = x.Key,
                    Quantity = x.Sum(z => z.ReceivedQuantity),
                    UnitCost = (decimal)x.FirstOrDefault().UnitPrice,
                    SellingPrice = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                    TotalCost = (decimal)x.Sum(z => z.Price),
                    BarCode = x.FirstOrDefault().Product.BarCode,
                    ProductId = x.FirstOrDefault().ProductInventoryId,
                    ProfitMade = 
                        CalculateProfit
                        (
                            (decimal)productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                            (decimal)x.FirstOrDefault()!.UnitPrice!,
                            (decimal)(openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0 + (x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0))
                        ),
                    Unit = x.FirstOrDefault().Unit,
                    RevenueMade = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price * (decimal)((x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0)),
                    VoucherNumber = x.FirstOrDefault().VoucherNumber,
                    OpeningQuantity = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    ClosingQuantity = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    OpeningStock = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                        * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                    ClosingStock = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                    * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                })
                .ToList();

            return new ServiceResponse<List<GroupedGrvItemsDto>>
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
            return new ServiceResponse<List<GroupedGrvItemsDto>>
            {
                IsSuccess = false,
                Message = $"Network Failed: {ex.Message}",
                Time = DateTime.Now,
            };
        }
    }

    public ServiceResponse<List<GroupedGrvItemsDto>> GetByMonthGrvItems(int month)
    {
        try
        {

            //last day of given month
            var lastDay = new DateTime(DateTime.Today.Year, month, DateTime.DaysInMonth(DateTime.Today.Year, month));
            //first day of given month
            var firstDay = new DateTime(DateTime.Today.Year, month, 1);

            var openingInventorySnap = _context.ProductInventorySnapshots!
                .Where(x => x.CreationTime.Date >= firstDay.Date.AddDays(-1) && x.SnapShotType == SnapShotEnum.CloseDay)
                .OrderByDescending(x => x.CreationTime)
                .LastOrDefault();

            var openingInventory = new List<ProductInventory>();

            if (openingInventorySnap != null)
            {
                openingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
            }

            var closingInventory = new List<ProductInventory>();

            if (month >= DateTime.Today.Month)
            {
                closingInventory = _context.ProductInventories!
                    .ToList();
            }
            else
            {
                var closingInventorySnap = _context.ProductInventorySnapshots!
                                            .Where(x => x.CreationTime.Date >= lastDay.Date && x.SnapShotType == SnapShotEnum.CloseDay)
                                            .OrderByDescending(x => x.CreationTime)
                                            .LastOrDefault();

                if (closingInventorySnap != null)
                {
                    closingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
                }
                else
                {
                    closingInventory = _context.ProductInventories!
                    .ToList();
                }
            }

            var products = _context.GoodsReceivedVoucherLines!
                 .Include(x => x.Product)
                 .Include(x => x.GoodsReceivedVoucher)
                 .Where(x => x.GoodsReceivedVoucher!.CreationTime.Month == month && x.GoodsReceivedVoucher.IsApproved)
                 .OrderBy(x => x.Product!.Name)
                 .ToList();
            
            var productPrices = _context.ProductPrices!.ToList();

            var _products = _mapper.Map<IEnumerable<GoodsReceivedVoucherLine>, IEnumerable<GoodsReceivedVoucherLineDto>>(products);

            var grouped = products
                .GroupBy(x => x.Product!.Name)
                .Select(x => new GroupedGrvItemsDto
                {
                    Category = x.FirstOrDefault()!.Product!.Category,
                    SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.Product!.SubCategoryId)!.Name,
                    SubCategoryId = x.FirstOrDefault()!.Product!.SubCategoryId,
                    ProductName = x.Key,
                    Quantity = x.Sum(z => z.ReceivedQuantity),
                    UnitCost = (decimal)x.FirstOrDefault()!.UnitPrice!,
                    SellingPrice = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                    ProfitMade = 
                        CalculateProfit
                        (
                            (decimal)productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                            (decimal)x.FirstOrDefault()!.UnitPrice!,
                            (decimal)(openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0 + (x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0))
                        ),
                    TotalCost = (decimal)x.Sum(z => z.Price)!,
                    BarCode = x.FirstOrDefault()!.Product!.BarCode,
                    ProductId = x.FirstOrDefault()!.ProductInventoryId,
                    Unit = x.FirstOrDefault()!.Unit,
                    RevenueMade = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price * (decimal)((x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0)),
                    VoucherNumber = x.FirstOrDefault()!.VoucherNumber,
                    OpeningQuantity = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    ClosingQuantity = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    OpeningStock = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!= null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand 
                                        * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                    ClosingStock = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand 
                                    * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                })
                .ToList();

            return new ServiceResponse<List<GroupedGrvItemsDto>>
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
            return new ServiceResponse<List<GroupedGrvItemsDto>>
            {
                IsSuccess = false,
                Message = $"Network Failed: {ex.Message}",
                Time = DateTime.Now,
            };
        }
    }

    private decimal CalculateProfit(decimal sellingPrice, decimal costPrice, decimal quantity)
    {
        return (sellingPrice * quantity) - (costPrice * quantity);
    }
    
    public ServiceResponse<List<GroupedGrvItemsDto>> GetGrvItemsByRange(DateTime start, DateTime end)
    {
        try
        {

            var openingInventorySnap = _context.ProductInventorySnapshots!
                .Where(x => x.CreationTime.Date >= start.Date.AddDays(-1) && x.SnapShotType == SnapShotEnum.CloseDay)
                .OrderByDescending(x => x.CreationTime)
                .LastOrDefault();

            var openingInventory = new List<ProductInventory>();

            if (openingInventorySnap != null)
            {
                openingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
            }

            var closingInventory = new List<ProductInventory>();

            if(start > DateTime.Today)
            {
                return new ServiceResponse<List<GroupedGrvItemsDto>>
                {
                    IsSuccess = false,
                    Message = $"Dates Out Of Range",
                    Time = DateTime.Now,
                };
            }

            if (end >= DateTime.Today)
            {
                closingInventory = _context.ProductInventories!
                    .ToList();
            }
            else
            {
                var closingInventorySnap = _context.ProductInventorySnapshots!
                                            .Where(x => x.CreationTime.Date >= end.Date && x.SnapShotType == SnapShotEnum.CloseDay)
                                            .OrderByDescending(x => x.CreationTime)
                                            .LastOrDefault();

                if (closingInventorySnap != null)
                {
                    closingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
                }
                else
                {
                    closingInventory = _context.ProductInventories!
                    .ToList();
                }
            }

            var products = _context.GoodsReceivedVoucherLines!
                 .Include(x => x.Product)
                 .Include(x => x.GoodsReceivedVoucher)
                 .Where(x => x.GoodsReceivedVoucher!.CreationTime.Date >= start.Date
                            && x.GoodsReceivedVoucher.CreationTime.Date <= end.Date
                            && x.GoodsReceivedVoucher.IsApproved)
                 .OrderBy(x => x.Product!.Name)
                 .ToList();
            
            var productPrices = _context.ProductPrices!.ToList();

            var _products = _mapper.Map<IEnumerable<GoodsReceivedVoucherLine>, IEnumerable<GoodsReceivedVoucherLineDto>>(products);

            var grouped = products
                .GroupBy(x => x.Product!.Name)
                .Select(x => new GroupedGrvItemsDto
                {
                    Category = x.FirstOrDefault().Product!.Category,
                    SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                    SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                    ProductName = x.Key,
                    Quantity = x.Sum(z => z.ReceivedQuantity),
                    UnitCost = (decimal)x.FirstOrDefault().UnitPrice,
                    RevenueMade = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price * (decimal)((x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0)),
                    SellingPrice = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                    ProfitMade = 
                        CalculateProfit
                        (
                            (decimal)productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                            (decimal)x.FirstOrDefault()!.UnitPrice!,
                            (decimal)(openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0 + (x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0))
                        ),
                    TotalCost = (decimal)x.Sum(z => z.Price),
                    BarCode = x.FirstOrDefault().Product.BarCode,
                    ProductId = x.FirstOrDefault().ProductInventoryId,
                    Unit = x.FirstOrDefault().Unit,
                    VoucherNumber = x.FirstOrDefault().VoucherNumber,
                    OpeningQuantity = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    ClosingQuantity = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    OpeningStock = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                        * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                    ClosingStock = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                    * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                })
                .ToList();

            return new ServiceResponse<List<GroupedGrvItemsDto>>
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
            return new ServiceResponse<List<GroupedGrvItemsDto>>
            {
                IsSuccess = false,
                Message = $"Network Failed: {ex.Message}",
                Time = DateTime.Now,
            };
        }
    }

    public ServiceResponse<List<GroupedGrvItemsDto>> GetTodayGrvItems()
    {
        try
        {
            var openingInventorySnap = _context.ProductInventorySnapshots!
                .Where(x => x.CreationTime.Date <= DateTime.Today.AddDays(-1) && x.SnapShotType == SnapShotEnum.CloseDay)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefault();

            var openingInventory = new List<ProductInventory>();

            if (openingInventorySnap != null)
            {
                openingInventory = JsonSerializer.Deserialize<List<ProductInventory>>(openingInventorySnap!.Inventory!);
            }

            var closingInventory = _context.ProductInventories!
                .ToList();

            var products = _context.GoodsReceivedVoucherLines!
                 .Include(x => x.Product)
                 .Include(x => x.GoodsReceivedVoucher)
                 .Where(x => x.GoodsReceivedVoucher!.CreationTime.Date == DateTime.Today && x.GoodsReceivedVoucher.IsApproved)
                 .OrderBy(x => x.Product!.Name)
                 .ToList();

            var productPrices = _context.ProductPrices!.ToList();

            var _products = _mapper.Map<IEnumerable<GoodsReceivedVoucherLine>, IEnumerable<GoodsReceivedVoucherLineDto>>(products);

            var grouped = products
                .GroupBy(x => x.Product!.Name)
                .Select(x => new GroupedGrvItemsDto
                {
                    Category = x.FirstOrDefault().Product!.Category,
                    SubCategory = _context.SubCategories!.FirstOrDefault(z => z.Id == x.FirstOrDefault().Product!.SubCategoryId)!.Name,
                    SubCategoryId = x.FirstOrDefault().Product!.SubCategoryId,
                    ProductName = x.Key,
                    Quantity = x.Sum(z => z.ReceivedQuantity),
                    UnitCost = (decimal)x.FirstOrDefault().UnitPrice,
                    TotalCost = (decimal)x.Sum(z => z.Price),
                    RevenueMade = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price * (decimal)(openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0 + (x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0)),
                    SellingPrice = productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                    BarCode = x.FirstOrDefault().Product.BarCode,
                    ProductId = x.FirstOrDefault().ProductInventoryId,
                    ProfitMade = 
                        CalculateProfit
                        (
                            (decimal)productPrices.FirstOrDefault(pp => pp.ProductInventoryId == x.FirstOrDefault().ProductInventoryId).Price,
                            (decimal)x.FirstOrDefault()!.UnitPrice!,
                            (decimal)(openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0 + (x.Sum(z => z.ReceivedQuantity)) - (closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0))
                        ),
                    Unit = x.FirstOrDefault().Unit,
                    VoucherNumber = x.FirstOrDefault().VoucherNumber,
                    OpeningQuantity = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    ClosingQuantity = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand : 0,
                    OpeningStock = openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                        ? openingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                        * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                    ClosingStock = closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId) != null
                                    ? closingInventory!.FirstOrDefault(z => z.Id == x.FirstOrDefault()!.ProductInventoryId)!.QuantityOnHand
                                    * (decimal)x.FirstOrDefault()!.UnitPrice! : 0,
                })
                .ToList();

            return new ServiceResponse<List<GroupedGrvItemsDto>>
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
            return new ServiceResponse<List<GroupedGrvItemsDto>>
            {
                IsSuccess = false,
                Message = $"Network Failed: {ex.Message}",
                Time = DateTime.Now,
            };
        }
    }

    private decimal GetCostOfGoodsSold(int? productId, decimal? cost, List<ProductInventory> openingInventory, List<ProductInventory> productInventories)
    {
        var openingProd = openingInventory.FirstOrDefault(x => x.Id == productId);
        var openingValue = (openingProd == null ? 0 : (decimal)openingProd.QuantityOnHand) * cost;
        var closingProd = productInventories.FirstOrDefault(x => x.Id == productId);
        var closingValue = (closingProd == null ? 0 :  closingProd!.QuantityOnHand) * cost;
        return (decimal)(openingValue - closingValue);
    }
}