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
    public class ProductInventorySnapShotService : IProductInventorySnapShotService
    {

        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductInventorySnapShotService> _logger;

        public ProductInventorySnapShotService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<ProductInventorySnapShotService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<IEnumerable<ProductInventorySnapshotDto>> CreateInventorySnapshots(InventorySnapShotSummaryDto snapShotDto)
        {
            try
            {
                var day = new InventorySnapShotLog();
                if (snapShotDto.SnapShotType == SnapShotEnum.StockTake)
                {
                    if(_context.StockTakeLogs!.Any(x => x.IsOpen && x.Department == snapShotDto.Department))
                    {
                        day = _context.InventorySnapShotLogs!
                        .FirstOrDefault(x => x.CreationTime.Date == DateTime.Now.Date && x.Department == snapShotDto.Department);
                    }
                    else
                    {
                       return new ServiceResponse<IEnumerable<ProductInventorySnapshotDto>>
                        {
                            IsSuccess = false,
                            Message = $"Stock Take Has Not Been Opened",
                            Time = DateTime.Now,
                        };
                    }
                }
                else
                {
                    day = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == DateTime.Now.Date);
                }


                if (day == null)
                {
                    var inventory = new List<ProductInventory>();

                    if (snapShotDto.SnapShotType == SnapShotEnum.StockTake)
                    {
                        inventory = _context.ProductInventories!
                            .Where(x => x.Category == snapShotDto.Department)
                            .ToList();
                    }
                    else
                    {
                        inventory = _context.ProductInventories!.ToList();
                    }

                    var summarizedInventory = inventory.Select(prod => new ProductInventory
                    {
                        Id = prod.Id,
                        BarCode = prod.BarCode,
                        Name = prod.Name,
                        QuantityOnHand = prod.QuantityOnHand,
                        Unit = prod.Unit,
                    });

                    if(snapShotDto.SnapShotType == SnapShotEnum.StockTake)
                    {
                        inventory.ForEach(prod =>
                        {
                            prod.QuantityOnHand = prod.StockCount;
                            prod.Status = prod.QuantityOnHand == 0 ? Status.OutOfStock : prod.QuantityOnHand > prod.IdealQuantity ? Status.LowStock : Status.InStock;
                        });
                        _context.ProductInventories!.UpdateRange(inventory);
                        _context.SaveChanges(); 
                    }

                    var snapShot = new ProductInventorySnapshot
                    {
                        CreationTime = DateTime.Now,
                        CreatorId = snapShotDto.UserId,
                        IsDeleted = false,
                        SnapShotType = snapShotDto.SnapShotType,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = snapShotDto.UserId,
                        Department = snapShotDto.SnapShotType == SnapShotEnum.StockTake ? snapShotDto.Department : null,
                        Inventory = JsonSerializer.Serialize(summarizedInventory)
                    };

                    _context.ProductInventorySnapshots!.Add(snapShot);
                    _context.SaveChanges();

                    var startDay = _context.InventorySnapShotLogs!.ToList().Count == 0 ? DateTime.Today : _context.InventorySnapShotLogs!
                        .OrderByDescending(x => x.CreationTime)
                        .First().CreationTime;

                    var log = new InventorySnapShotLog
                    {
                        StartDay = startDay,
                        CreationTime = DateTime.Now,
                        CreatorId = snapShotDto.UserId,
                        IsDeleted = false,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = snapShotDto.UserId,
                    };

                    _context.InventorySnapShotLogs!.Add(log);
                    _context.SaveChanges();

                    return new ServiceResponse<IEnumerable<ProductInventorySnapshotDto>>
                    {
                        IsSuccess = true,
                        Message = $"SnapShot Created Successifully",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    return new ServiceResponse<IEnumerable<ProductInventorySnapshotDto>>
                    {
                        IsSuccess = false,
                        Message = snapShotDto.SnapShotType == SnapShotEnum.StockTake ? $"Stock Take Has Already Been Closed" : $"Day Has Already Been Closed",
                        Time = DateTime.Now,
                    };
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating snapshot ");
                return new ServiceResponse<IEnumerable<ProductInventorySnapshotDto>>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductInventorySnapshotDto> GetSnapShotByDate(DateTime date)
        {
            try
            {
                var snapShots = _context.ProductInventorySnapshots!
                    .FirstOrDefault(c => c.CreationTime.Date == date.Date);


                var log = _context.InventorySnapShotLogs!
                    .FirstOrDefault(x => x.CreationTime.Date == date.Date);

                var _log = _mapper.Map<ProductInventorySnapshot, ProductInventorySnapshotDto>(snapShots!);


                return new ServiceResponse<ProductInventorySnapshotDto>
                {
                    Data = _log,
                    IsSuccess = true,
                    Message = $"SnapShot Created Successifully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while geting snapshot ");
                return new ServiceResponse<ProductInventorySnapshotDto>
                {
                    IsSuccess = false,
                    Message = $"Inventory Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
