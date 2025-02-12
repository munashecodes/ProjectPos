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
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class ProductInventoryService : IProductInventoryService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductInventoryService> _logger;

        public ProductInventoryService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<ProductInventoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<ProductInventoryDto> Create(ProductInventoryDto inventoryDto)
        {
            try
            {
                var _prod = _context.ProductInventories!.Any(x => x.BarCode == inventoryDto.BarCode);

                if (_prod!)
                {
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = $"Barcode {inventoryDto.BarCode} already exists. Cannot have duplicate barcodes",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var inventory = _mapper.Map<ProductInventoryDto, ProductInventory>(inventoryDto);
                    inventory.Status = Status.OutOfStock;
                    inventory.QuantityOnHand = 0;
                    inventory.QuantityOnShelf = 0;
                    var _inventory = _context.ProductInventories!.Add(inventory);
                    _context.SaveChanges();
                    
                    var product =_context.ProductInventories!
                        .Include(x => x!.SubCategory)
                        .FirstOrDefault(p => p.Id == _inventory.Entity.Id);
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        Data = _mapper.Map<ProductInventory, ProductInventoryDto>(product),
                        IsSuccess = true,
                        Message = "Product Added Successfully",
                        Time = DateTime.Now,
                    };
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating inventory");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Product Addition Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task UpdateProductInventoryStatus()
        {
            try
            {
                var products = await _context.ProductInventories.ToListAsync();
                foreach (var product in products)
                {
                    if (product.QuantityOnHand <= 0)
                    {
                        product.Status = Status.OutOfStock;
                    } else if (product.QuantityOnHand < product.IdealQuantity)
                    {
                        product.Status = Status.LowStock;
                    }
                    else
                    {
                        product.Status = Status.InStock;
                    }
                }
                _logger.LogInformation("Saving Product Inventory Status Changes");
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error product updating status");
            }
        }

        public ServiceResponse<ProductInventoryDto> Delete(int id)
        {
            try
            {
                var inventory = _context.ProductInventories!
                    .FirstOrDefault(x => x.Id == id);

                if (inventory == null)
                {
                    _logger.LogError($"Product with id: {id} does not exist");
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = "Product Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    inventory.IsDeleted = true;
                    _context.ProductInventories!.Update(inventory!);
                    _context.SaveChanges();
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = true,
                        Message = $"Product {inventory!.Name} was deleted successfully",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<ProductInventoryDto>>> GenerateInventory()
        {
            try
            {
                var products = await _context.ProductInventories!
                    .ToListAsync();

                var inventories = products.Select(prod => new ProductInventory
                {
                    BarCode = prod.BarCode,
                    Name = prod.Name,
                    Grade = prod.Grade,
                    Cost = 0,
                    MarkUp = 30,
                    IdealQuantity = 0,
                    QuantityOnHand = 0,
                    QuantityOnShelf = 0,
                    Unit = Unit.Units,
                    Status = Status.InStock,
                    CreatorId = 1,
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = 1,
                    IsDeleted = false,
                    PLUCode = prod.BarCode == null ? null : prod.BarCode!.Substring(2, 4),
                    Flag = prod.Category == Category.FreshProduce ? 20 : 0,
                    IsWeighted = prod.Category == Category.FreshProduce ? true : false,
                });

                await _context.ProductInventories!.AddRangeAsync(inventories);
                await _context.SaveChangesAsync();

                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    IsSuccess = true,
                    Message = "Inventory Generated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating inventory");
                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<ProductInventoryDto>> GetAll()
        {
            try
            {
                var inventories = _context.ProductInventories!
                    .Include(x => x!.SubCategory)
                    .OrderBy(x => x.Name)
                    .Where(c  => c.IsDeleted == false)
                    .ToList();
                //var _inventories = _mapper.Map<List<ProductInventory>, List<ProductInventoryDto>>(inventories);

                var _inventories = _mapper.Map<List<ProductInventoryDto>>(inventories);
                _inventories.ForEach(inv =>
                {
                    inv.ProductPrice = _mapper.Map<ProductPrice, ProductPriceDto>(
                        _context.ProductPrices!
                        .OrderBy(a => a.CreationTime)
                        .LastOrDefault(a => a.ProductInventoryId == inv.Id)!
                        );
                });

                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    Data = _inventories,
                    IsSuccess = true,
                    Message = $"Found {_inventories.Count} Products",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all inventories");
                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<IEnumerable<ProductInventoryDto>> GetAllToSell()
        {
            try
            {
                var inventories = _context.ProductInventories!
                    .Include(x => x!.SubCategory)
                    .OrderBy(x => x.Name)
                    .Where(c => c.IsDeleted == false)
                    .ToList();
                //var _inventories = _mapper.Map<List<ProductInventory>, List<ProductInventoryDto>>(inventories);

                var _inventories = _mapper.Map<List<ProductInventoryDto>>(inventories);

                _inventories.ForEach(inv =>
                {
                    inv.ProductPrice = _mapper.Map<ProductPrice, ProductPriceDto>(
                        _context.ProductPrices!
                        .OrderBy(a => a.CreationTime)
                        .LastOrDefault(a => a.ProductInventoryId == inv.Id)!
                        );
                });

                var res = _inventories.Where(i => i.ProductPrice != null);

                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    Data = res,
                    IsSuccess = true,
                    Message = $"Found  ProductInventories",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all inventories");
                return new ServiceResponse<IEnumerable<ProductInventoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductInventoryDto> GetById(int id)
        {
            try
            {
                var inventory = _context.ProductInventories
                    .FirstOrDefault(c => c.Id == id && c.IsDeleted == false);

                

                if (inventory == null)
                {
                    _logger.LogError($"ProductInventory with id: {id} does not exist");
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = $"inventory {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var price = _context.ProductPrices
                    .OrderBy(x => x.CreationTime)
                    .LastOrDefault(x => x.ProductInventoryId == inventory.Id);

                    if (price == null)
                    {
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            IsSuccess = false,
                            Message = $"Product Price for {inventory.Name} Was Not Found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _inventory = _mapper.Map<ProductInventory, ProductInventoryDto>(inventory);
                        var _price = _mapper.Map<ProductPrice, ProductPriceDto>(price);

                        _inventory.ProductPrice = _price;
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            Data = _inventory,
                            IsSuccess = true,
                            Message = $"inventory {inventory.Name} Was Found",
                            Time = DateTime.Now,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting inventory by id");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductInventoryDto> GetByPlu(string plu)
        {
            try
            {
                var inventory = _context.ProductInventories!
                    .FirstOrDefault(c => c.PLUCode == plu && c.IsDeleted == false);

                if (inventory == null)
                {
                    _logger.LogError($"ProductInventory with id: {plu} does not exist");
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = $"inventory {plu} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var price = _context.ProductPrices!
                    .OrderBy(x => x.CreationTime)
                    .LastOrDefault(x => x.ProductInventoryId == inventory!.Id);
                    if (price == null) {
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            IsSuccess = false,
                            Message = $"Product Price for {inventory.Name} Was Not Found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _inventory = _mapper.Map<ProductInventory, ProductInventoryDto>(inventory);
                        var _price = _mapper.Map<ProductPrice, ProductPriceDto>(price);

                        _inventory.ProductPrice = _price;
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            Data = _inventory,
                            IsSuccess = true,
                            Message = $"inventory {inventory.Name} Was Found",
                            Time = DateTime.Now,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting inventory by id");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductInventoryDto> GetByName(string name)
        {
            try
            {
                var inventories = _context.ProductInventories!
                    .FirstOrDefault(c => c.BarCode == name && c.IsDeleted == false);

                if (inventories == null)
                {
                    _logger.LogError($"ProductInventory with Bar Code: {name} does not exist");
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = $"ProductInventories with Bar Code {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var price = _context.ProductPrices!
                    .OrderBy(x => x.CreationTime)
                    .LastOrDefault(x => x.ProductInventoryId == inventories!.Id);
                    if (price == null) {
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            IsSuccess = false,
                            Message = $"Product Price for {inventories.Name} Was Not Found",
                            Time = DateTime.Now,
                        };
                    }
                    else
                    {
                        var _inventory = _mapper.Map<ProductInventory, ProductInventoryDto>(inventories);
                        var _price = _mapper.Map<ProductPrice, ProductPriceDto>(price);

                        _inventory.ProductPrice = _price;
                        return new ServiceResponse<ProductInventoryDto>
                        {
                            Data = _inventory,
                            IsSuccess = true,
                            Message = $"inventory {inventories.Name} Was Found",
                            Time = DateTime.Now,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting inventory by name");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductInventoryDto> Update(ProductInventoryDto inventoryDto)
        {
            try
            {
                var _prod = _context.ProductInventories!.Any(x => x.BarCode == inventoryDto.BarCode && inventoryDto.Id != x.Id);

                if (_prod!)
                {
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        IsSuccess = false,
                        Message = $"Barcode {inventoryDto.BarCode} already exists. Cannot have duplicate barcodes",
                        Time = DateTime.Now,
                    };
                }
                else
                {

                    var inventory = _mapper.Map<ProductInventoryDto, ProductInventory>(inventoryDto);

                    inventory.Status = inventory.QuantityOnHand <= 0 ? Status.OutOfStock : inventory.QuantityOnHand > inventory.IdealQuantity ? Status.InStock : Status.LowStock;
                    var _inventory = _context.ProductInventories!.Update(inventory);
                    _context.SaveChanges();
                    
                    var product = _context.ProductInventories!
                        .Include(pi => pi.SubCategory)
                        .FirstOrDefault(x => x.Id == _inventory.Entity.Id);
                    return new ServiceResponse<ProductInventoryDto>
                    {
                        Data = _mapper.Map<ProductInventory, ProductInventoryDto>(product),
                        IsSuccess = true,
                        Message = "Product Updated Successfully",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating inventory");
                return new ServiceResponse<ProductInventoryDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ProductInventoryDto>> UpdateRange(List<ProductInventoryDto> inventory)
        {
            try
            {
                var filterd = inventory.Where(x => x.StockCount != 0 && x.StockCount != null).ToList();
                
                if(filterd.Count == 0)
                {
                    return new ServiceResponse<List<ProductInventoryDto>>
                    {
                        IsSuccess = false,
                        Message = "Inventory not UpDated Successfully",
                        Time = DateTime.Now,
                    };
                }
                var _inventory = _mapper.Map<List<ProductInventoryDto>, List<ProductInventory>>(inventory);
                var inv = _context.ProductInventories!
                    .Where(x => _inventory.Select(a => a.Id).Contains(x.Id))
                    .ToList();

                inv.ForEach(prod =>
                {
                    var _prod = inventory!
                        .FirstOrDefault(x => x.Id == prod.Id);
                    var count = _prod!.StockCount;
                    prod.StockCount = count == null? prod.StockCount : (prod.StockCount + count);
                    prod.SubCategoryId = _prod.SubCategoryId;
                });

                _context.ProductInventories!.UpdateRange(inv);
                _context.SaveChanges();
                return new ServiceResponse<List<ProductInventoryDto>>
                {
                    IsSuccess = true,
                    Message = "Inventory UpDated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating inventory");
                return new ServiceResponse<List<ProductInventoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Inventory Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockTakeReportDto>>> GenerateStockTakeReport(Category department)
        {
            try
            {

                var date = _context.StockTakeLogs!
                    .OrderByDescending(x => x.LastModificationTime)
                    .FirstOrDefault(x => x.Department == department && x.IsOpen == false)?.LastModificationTime ?? DateTime.Today;

                var stock = await _context.ProductInventories!
                    .Include(x => x!.SubCategory)
                    .Where(x => x.Category == department && x.IsDeleted == false)
                    .ToListAsync();

                var openingStockJson = await _context.ProductInventorySnapshots!
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.CreationTime.Date == date.Date 
                                            && x.SnapShotType == SnapShotEnum.StockTake
                                            && x.Department == department);

                var openingStock = openingStockJson == null ? new List<ProductInventoryDto>() 
                    : JsonSerializer.Deserialize<List<ProductInventoryDto>>(openingStockJson.Inventory!);

                var receivedStock = await _context.GoodsReceivedVoucherLines!
                    .Include(x => x.GoodsReceivedVoucher)
                    .OrderByDescending(x => x.GoodsReceivedVoucher!.CreationTime)
                    .Where(x => x.GoodsReceivedVoucher!.CreationTime > date)
                    .ToListAsync();

                var soldStock = await _context.SalesOrderItems!
                    .Include(x => x.SalesOrder)
                    .OrderByDescending(x => x.SalesOrder!.CreationTime)
                    .Where(x => x.SalesOrder!.CreationTime > date)
                    .ToListAsync();

                var damagedStock = await _context.StockMovements!
                    .Where(x => x.CreationTime > date && x.TransactionType == TransactionType.Breakage)
                    .OrderByDescending(x => x.CreationTime)
                    .Select(x => new StockMovement
                    {
                        ProductInventoryId = x.ProductInventoryId,
                        Quantity = x.Quantity,
                        TransactionType = x.TransactionType,
                    })
                    .ToListAsync();

                var stockTake = stock.Select(x => new StockTakeReportDto
                {
                    ProductId = x.Id,
                    BarCode = x.BarCode,
                    ProductName = x.Name,
                    SubCategory = x.SubCategory.Name,
                    OpeningStock = openingStock!.FirstOrDefault(a => a.Id == x.Id)?.QuantityOnHand ?? 0,
                    ReceivedQuantity = receivedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.ReceivedQuantity) ?? 0,
                    DamagedQuantity = damagedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.Quantity) ?? 0,
                    ReturnedQuantity = 0,
                    SoldQuantity = soldStock.Where(a => a.ProductId == x.Id).Sum(a => a.Quantity) ?? 0,
                    ClosingStock = (openingStock!.FirstOrDefault(a => a.Id == x.Id)?.QuantityOnHand ?? 0)
                                    + (receivedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.ReceivedQuantity) ?? 0)
                                    - (soldStock.Where(a => a.ProductId == x.Id).Sum(a => a.Quantity) ?? 0)
                                    - (damagedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.Quantity) ?? 0),
                    QuantityOnHand = (decimal)x.QuantityOnHand!,
                    QuantityOnShelf = (decimal)x.StockCount!,
                    Variance = (decimal)x.StockCount! - (decimal)x.QuantityOnHand!,
                    Unit = x.Unit
                });

                return new ServiceResponse<IEnumerable<StockTakeReportDto>>
                {
                    Data = stockTake,
                    IsSuccess = true,
                    Message = "Stock Take Report Generated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating stock take report");
                return new ServiceResponse<IEnumerable<StockTakeReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockValueReportDto>>> GenerateStockValueReport(Category department)
        {
            try
            {
                var date = _context.StockTakeLogs!
                    .OrderByDescending(x => x.LastModificationTime)
                    .FirstOrDefault(x => x.Department == department && x.IsOpen == false)?.LastModificationTime ?? DateTime.Today;

                var stock = await _context.ProductInventories!
                    .Include(x => x!.SubCategory)
                    .Where(x => x.Category == department && x.IsDeleted == false)
                    .ToListAsync();

                var openingStockJson = await _context.ProductInventorySnapshots!
                    .OrderByDescending(x => x.CreationTime)
                    .FirstOrDefaultAsync(x => x.CreationTime.Date == date.Date
                                            && x.SnapShotType == SnapShotEnum.StockTake
                                            && x.Department == department);

                var openingStock = openingStockJson == null ? new List<ProductInventoryDto>()
                    : JsonSerializer.Deserialize<List<ProductInventoryDto>>(openingStockJson.Inventory!);

                var receivedStock = await _context.GoodsReceivedVoucherLines!
                    .Include(x => x.GoodsReceivedVoucher)
                    .OrderByDescending(x => x.GoodsReceivedVoucher!.CreationTime)
                    .Where(x => x.GoodsReceivedVoucher!.CreationTime > date)
                    .ToListAsync();

                var soldStock = await _context.SalesOrderItems!
                    .Include(x => x.SalesOrder)
                    .OrderByDescending(x => x.SalesOrder!.CreationTime)
                    .Where(x => x.SalesOrder!.CreationTime > date)
                    .ToListAsync();

                var damagedStock = await _context.StockMovements!
                    .Where(x => x.CreationTime > date && x.TransactionType == TransactionType.Breakage)
                    .OrderByDescending(x => x.CreationTime)
                    .Select(x => new StockMovement
                    {
                        ProductInventoryId = x.ProductInventoryId,
                        Quantity = x.Quantity,
                        TransactionType = x.TransactionType,
                    })
                    .ToListAsync();

                var stockTake = stock.Select(x => new StockTakeReportDto
                {
                    ProductId = x.Id,
                    BarCode = x.BarCode,
                    ProductName = x.Name,
                    SubCategory = x.SubCategory.Name,
                    OpeningStock = openingStock!.FirstOrDefault(a => a.Id == x.Id)?.QuantityOnHand ?? 0,
                    ReceivedQuantity = receivedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.ReceivedQuantity) ?? 0,
                    DamagedQuantity = damagedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.Quantity) ?? 0,
                    ReturnedQuantity = 0,
                    SoldQuantity = soldStock.Where(a => a.ProductId == x.Id).Sum(a => a.Quantity) ?? 0,
                    ClosingStock = (openingStock!.FirstOrDefault(a => a.Id == x.Id)?.QuantityOnHand ?? 0)
                                    + (receivedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.ReceivedQuantity) ?? 0)
                                    -( soldStock.Where(a => a.ProductId == x.Id).Sum(a => a.Quantity) ?? 0)
                                    - (damagedStock.Where(a => a.ProductInventoryId == x.Id).Sum(a => a.Quantity) ?? 0),
                    QuantityOnHand = (decimal)x.QuantityOnHand!,
                    QuantityOnShelf = (decimal)x.StockCount!,
                    Variance = (decimal)x.StockCount! - (decimal)x.QuantityOnHand!,
                    Unit = x.Unit
                });

                var stockValue = new List<StockValueReportDto>();

                foreach (var prod in stockTake)
                {
                    var _price = _context.ProductPrices!
                        .OrderBy(a => a.CreationTime)
                        .LastOrDefault(a => a.ProductInventoryId == prod.ProductId);

                    var price = _price == null ? 0 : _price.Price;

                    var value = new StockValueReportDto
                    {
                        BarCode = prod.BarCode,
                        ProductId = prod.ProductId,
                        ProductName = prod.ProductName,
                        SubCategory = prod.SubCategory,
                        OpeningStock = prod.OpeningStock * (decimal)price,
                        ReceivedStock = prod.ReceivedQuantity * (decimal)price,
                        DamagedStock = prod.DamagedQuantity * (decimal)price,
                        ReturnedStock = prod.ReturnedQuantity * (decimal)price,
                        SoldStock = prod.SoldQuantity * (decimal)price,
                        ClosingStock = prod.ClosingStock * (decimal)price,
                        StockOnHand = prod.QuantityOnHand * (decimal)price,
                        StockOnShelf = prod.QuantityOnShelf * (decimal)price,
                        Variance = prod.Variance * (decimal)price,
                    };

                    stockValue.Add(value);
                }

                return new ServiceResponse<IEnumerable<StockValueReportDto>>
                {
                    Data = stockValue,
                    IsSuccess = true,
                    Message = "Stock Value Report Generated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating stock value report");
                return new ServiceResponse<IEnumerable<StockValueReportDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<StockTakeLogDto>> CreateStockTakeLog(StockTakeLogDto log)
        {
            try
            {
                var _log = _mapper.Map<StockTakeLogDto, StockTakeLog>(log);
                _log.CreationTime = DateTime.Now;
                _log.IsOpen = true;

                var inventory = await _context.ProductInventories!
                    .Where(x => x.Category == log.Department)
                    .ToListAsync();

                inventory.ForEach(prod =>
                {
                    prod.StockCount = 0;
                    prod.Status = prod.QuantityOnHand == 0 ? Status.OutOfStock : prod.QuantityOnHand > prod.IdealQuantity ? Status.InStock : Status.LowStock;
                });
                _context.ProductInventories!.UpdateRange(inventory);

                var _stock = await _context.StockTakeLogs!.AddAsync(_log);
                _context.SaveChanges();

                return new ServiceResponse<StockTakeLogDto>
                {
                    Data = _mapper.Map<StockTakeLog, StockTakeLogDto>(_stock.Entity),
                    IsSuccess = true,
                    Message = "Stock Take Log Created Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating stock take log");
                return new ServiceResponse<StockTakeLogDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<StockTakeLogDto>> CloseStockTakeLog(StockTakeLogDto log)
        {
            try
            {
                var _log = await _context.StockTakeLogs!.FirstOrDefaultAsync(x => x.IsOpen && x.Department == log.Department);

                if(_log == null)
                {
                    return new ServiceResponse<StockTakeLogDto>
                    {
                        IsSuccess = false,
                        Message = "Stock Take Log Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _log.IsOpen = false;
                    _log.LastModificationTime = DateTime.Now;
                    _log.LastModifierUserId = log.CreatorId;

                    var _stock = _context.StockTakeLogs!.Update(_log);
                    _context.SaveChanges();

                    return new ServiceResponse<StockTakeLogDto>
                    {
                        Data = _mapper.Map<StockTakeLog, StockTakeLogDto>(_stock.Entity),
                        IsSuccess = true,
                        Message = "Stock Take Log Closed Successfully",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating stock take log");
                return new ServiceResponse<StockTakeLogDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<StockTakeLogDto>> CheckStockTakeLog(Category department)
        {
            try
            {
                var _log = await _context.StockTakeLogs!.AnyAsync(x => x.IsOpen && x.Department == department);

                if(_log)
                {
                    return new ServiceResponse<StockTakeLogDto>
                    {
                        IsSuccess = true,
                        Message = "Stock Take Log Is Open",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    return new ServiceResponse<StockTakeLogDto>
                    {
                        IsSuccess = false,
                        Message = "There is no open Stock Take",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating stock take log");
                return new ServiceResponse<StockTakeLogDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }


    }
}
