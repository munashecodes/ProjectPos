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
    public class StockMovementService : IStockMovementService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StockMovementService> _logger;

        public StockMovementService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<StockMovementService> logger) 
        { 
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<StockMovementLogDto>> AddAsync(StockMovementLogDto stockMovement)
        {
            try
            {
                var stockMovementEntity = _mapper.Map<StockMovementLog>(stockMovement);
                var res = await _context.StockMovementLogs!.AddAsync(stockMovementEntity);
                await _context.SaveChangesAsync();

                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = _mapper.Map<StockMovementLogDto>(res.Entity),
                    Message = "Stock Movement added successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<List<StockMovementDto>>> AddRangeAsync(List<StockMovementDto> stockMovement)
        {
            //create try catch block
            try
            {
                //map the dto to the entity
                var stockMovementEntity = _mapper.Map<List<StockMovement>>(stockMovement);

                //add the entity to the context asynchronusly
                await _context.StockMovements!.AddRangeAsync(stockMovementEntity);

                //loop through the stockmovements and update the productinventory quantity
                foreach (var item in stockMovement)
                {
                    //get the productinventory from the context
                    var productInventory = await _context.ProductInventories!.FirstOrDefaultAsync(x => x.Id == item.ProductInventoryId);

                    //edit the productinventory quantity as per the stockmovement
                    productInventory!.QuantityOnHand += item.Quantity;
                    productInventory.Status = productInventory.QuantityOnHand == 0 ? Status.OutOfStock : productInventory.QuantityOnHand > productInventory.IdealQuantity ? Status.InStock : Status.LowStock;

                    if (productInventory.QuantityOnHand < 0)
                    {
                        //return the result
                        return new ServiceResponse<List<StockMovementDto>>
                        {
                            Data = null,
                            Message = $"Stock Movements failed to add. Quantity on hand cannot be less than 0. {productInventory.Name} quantity has got to {productInventory.QuantityOnHand}",
                            IsSuccess = false
                        };
                    }
                    else if (productInventory.QuantityOnHand > productInventory.IdealQuantity)
                    {
                        productInventory.Status = Status.LowStock;
                    }
                    else
                    {
                        productInventory.Status = Status.InStock;
                    }

                    //update the productinventory in the context asynchronusly
                    _context.ProductInventories.Update(productInventory);
                }

                //save the changes in the context asynchronusly
                await _context.SaveChangesAsync();

                //return the result
                return new ServiceResponse<List<StockMovementDto>>
                {
                    Data = _mapper.Map<List<StockMovementDto>>(stockMovementEntity),
                    Message = "Stock Movements added successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                //log the exception
                _logger.LogError(ex, ex.Message);

                //return the exception
                return new ServiceResponse<List<StockMovementDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<StockMovementLogDto>> Approve(int id, int userId)
        {
            try
            {
                var stockMovement = await _context.StockMovementLogs!
                    .Include(x => x.StockMovements)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var products = new List<ProductInventory>();

                //loop through the stockmovements and update the productinventory quantity
                foreach (var item in stockMovement!.StockMovements!)
                {
                    //get the productinventory from the context
                    var productInventory = await _context.ProductInventories!.FirstOrDefaultAsync(x => x.Id == item.ProductInventoryId);


                    if (productInventory.QuantityOnHand < (item.Quantity * -1))
                    {
                        //return the result
                        return new ServiceResponse<StockMovementLogDto>
                        {
                            Data = null,
                            Message = $"Stock Movements failed to add. Quantity on hand cannot be less than 0. {productInventory.Name} quantity on hand is {productInventory.QuantityOnHand}",
                            IsSuccess = false
                        };
                    }
                    else
                    {
                        //edit the productinventory quantity as per the stockmovement
                        productInventory!.QuantityOnHand += item.Quantity;
                        productInventory.Status = productInventory.QuantityOnHand == 0 ? Status.OutOfStock : productInventory.QuantityOnHand > productInventory.IdealQuantity ? Status.InStock : productInventory.QuantityOnHand < 0 ? Status.OutOfStock : Status.LowStock;
                        item.IsAuthorised = true;
                        item.AuthorisedById = userId;

                        //update the productinventory in the context asynchronusly
                        products.Add(productInventory);
                    }
                }

                stockMovement.IsAuthorised = true;
                stockMovement.AuthorisedById = userId;

                //update the productinventory in the context asynchronusly
                _context.ProductInventories!.UpdateRange(products);
                _context.StockMovementLogs!.Update(stockMovement);

                //save the changes in the context asynchronusly
                await _context.SaveChangesAsync();

                //return the result
                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = _mapper.Map<StockMovementLogDto>(stockMovement),
                    Message = "Stock Movements approved successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public Task<ServiceResponse<StockMovementLogDto>> DeleteAsync(int id, int userId)
        {
            try
            {

                var stockMovement = _context.StockMovementLogs!.FirstOrDefault(x => x.Id == id);
                stockMovement.IsDeleted = true;
                stockMovement.DeleterId = userId;
                stockMovement.DeletionTime = DateTime.Now;

                var res = _context.StockMovementLogs.Update(stockMovement);
                _context.SaveChanges();

                return Task.FromResult(new ServiceResponse<StockMovementLogDto>
                {
                    Data = _mapper.Map<StockMovementLogDto>(res.Entity),
                    Message = "Stock Movement deleted successfully",
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.FromResult(new ServiceResponse<StockMovementLogDto>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                });
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllAsync()
        {
            try
            {
                var stockMovements = await _context.StockMovementLogs!
                    .Include(x => x.StockMovements)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();
                var _stockMovements = _mapper.Map<List<StockMovementLogDto>>(stockMovements);

                _stockMovements.ForEach(y =>
                {
                    y.CreatedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.CreatorId)!.FullName;
                    y.AuthorisedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.AuthorisedById)!.FullName;
                });

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = _stockMovements,
                    Message = "Stock Movements retrieved successfully",
                    IsSuccess = true,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByDateAsync(DateTime date)
        {
            try
            {
                var stockMovements = await _context.StockMovementLogs!
                    .Include(x => x.StockMovements)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.IsDeleted == false && x.CreationTime.Date == date.Date)
                    .ToListAsync();

                var _stockMovements = _mapper.Map<List<StockMovementLogDto>>(stockMovements);

                _stockMovements.ForEach(y =>
                {
                    y.CreatedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.CreatorId)!.FullName;
                    y.AuthorisedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.AuthorisedById)!.FullName;
                });

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = _stockMovements,
                    Message = "Stock Movements retrieved successfully",
                    IsSuccess = true,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var stockMovements = await _context.StockMovementLogs!
                .Include(x => x.StockMovements)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.IsDeleted == false && x.CreationTime.Date > startDate.Date && x.CreationTime.Date < endDate.Date)
                    .ToListAsync();

                var _stockMovements = _mapper.Map<List<StockMovementLogDto>>(stockMovements);

                _stockMovements.ForEach(y =>
                {
                    y.CreatedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.CreatorId)!.FullName;
                    y.AuthorisedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.AuthorisedById)!.FullName;
                });

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = _stockMovements,
                    Message = "Stock Movements retrieved successfully",
                    IsSuccess = true,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByMonthAsync(int month)
        {
            try
            {
                var year = DateTime.Now.Year;
                var stockMovements = await _context.StockMovementLogs!
                .Include(x => x.StockMovements)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.IsDeleted == false && x.CreationTime.Month == month && x.CreationTime.Year == year)
                    .ToListAsync();

                var _stockMovements = _mapper.Map<List<StockMovementLogDto>>(stockMovements);

                _stockMovements.ForEach(y =>
                {
                    y.CreatedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.CreatorId)!.FullName;
                    y.AuthorisedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.AuthorisedById)!.FullName;
                });

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = _stockMovements,
                    Message = "Stock Movements retrieved successfully",
                    IsSuccess = true,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllTodayAsync()
        {
            try
            {
                var stockMovements = await _context.StockMovementLogs!
                    .Include(x => x.StockMovements)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.IsDeleted == false && x.CreationTime.Date == DateTime.Today)
                    .ToListAsync();

                var _stockMovements = _mapper.Map<List<StockMovementLogDto>>(stockMovements);

                _stockMovements.ForEach(y =>
                {
                    y.CreatedBy = _context.SystemUsers!.FirstOrDefault(z => z.Id == y.CreatorId)!.FullName;
                    y.AuthorisedBy = y.IsAuthorised == true ? _context.SystemUsers!.FirstOrDefault(z => z.Id == y.AuthorisedById)!.FullName : "Not Authorized";
                });

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = _stockMovements,
                    Message = "Stock Movements retrieved successfully",
                    IsSuccess = true,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<IEnumerable<StockMovementLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public Task<ServiceResponse<StockMovementLogDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<StockMovementLogDto>> UpdateAsync(StockMovementLogDto stockMovement)
        {
            try
            {
                var stockMovementEntity = _mapper.Map<StockMovementLog>(stockMovement);
                var res = _context.StockMovementLogs!.Update(stockMovementEntity);
                await _context.SaveChangesAsync();

                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = _mapper.Map<StockMovementLogDto>(res.Entity),
                    Message = "Stock Movement updated successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new ServiceResponse<StockMovementLogDto>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }
    }
}
