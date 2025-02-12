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
    public class GoodsReceivedVoucherService : IGoodsReceivedVoucherService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GoodsReceivedVoucherService> _logger;

        public GoodsReceivedVoucherService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<GoodsReceivedVoucherService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<GoodsReceivedVoucherDto> Create(GoodsReceivedVoucherDto companyDto)
        {
            try
            {
                var grv = _mapper.Map<GoodsReceivedVoucherDto, GoodsReceivedVoucher>(companyDto);

                grv.Status = grv.AmountDue == grv.Value ? OrderPaymentStatus.NotPaid : grv.AmountDue > 0 ? OrderPaymentStatus.PartiallyPaid : OrderPaymentStatus.Paid;
                grv.IsPaid = grv.AmountDue > 0 ? false : true;


                // Fetch Purchase Order
                var order = _context.PurchaceOrders
                    .Include(x => x.PurchaceOrderItems)
                    .FirstOrDefault(x => x.Id == grv.OrderNumber);

                if (order != null)
                {
                    // Fetch related GRVs and calculate received quantity
                    var grvs = _context.GoodsReceivedVouchers
                        .Where(x => x.OrderNumber == grv.OrderNumber)
                        .Include(x => x.ReceivedItems)
                        .ToList();

                    var rcvdQty = grvs.Sum(g => g.ReceivedItems.Sum(i => i.ReceivedQuantity));
                    var orderedQty = order.PurchaceOrderItems.Sum(x => x.Quantity);

                    //order.IsOpen = rcvdQty < orderedQty ? true : false;
                    order.IsReceived = true;

                    _context.PurchaceOrders.Update(order);
                }

                var _grv = _context.GoodsReceivedVouchers!.Update(grv);
                _context.SaveChanges();

                var res = _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .FirstOrDefault(x => x.Id == _grv.Entity.Id);

                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    Data = _mapper.Map<GoodsReceivedVoucher, GoodsReceivedVoucherDto>(_grv.Entity),
                    IsSuccess = true,
                    Message = "Goods Received Voucher Created Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating grv");
                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    IsSuccess = false,
                    Message = $"Voucher Creation Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<GoodsReceivedVoucherDto> Delete(int id)
        {
            try
            {
                var grv = _context.GoodsReceivedVouchers!.FirstOrDefault(x => x.Id == id);

                if (grv == null)
                {
                    _logger.LogError($"Goods Received Voucher with id: {id} does not exist");
                    return new ServiceResponse<GoodsReceivedVoucherDto>
                    {
                        IsSuccess = false,
                        Message = "Goods Received Voucher Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.GoodsReceivedVouchers!.Remove(grv!);
                    _context.SaveChanges();
                    return new ServiceResponse<GoodsReceivedVoucherDto>
                    {
                        IsSuccess = true,
                        Message = $"Goods Received Voucher - {grv.Supplier!.Name} was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting grv");
                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<GoodsReceivedVoucherDto>> GetAll()
        {
            try
            {
                var grvs = _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .ToList();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<GoodsReceivedVoucherDto> GetById(int id)
        {
            try
            {
                var grv = _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .FirstOrDefault(c => c.Id == id);

                if (grv == null)
                {
                    _logger.LogError($"Goods Received Voucher with id: {id} does not exist");
                    return new ServiceResponse<GoodsReceivedVoucherDto>
                    {
                        IsSuccess = false,
                        Message = $"Goods Received Voucher - {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _grv = _mapper.Map<GoodsReceivedVoucher, GoodsReceivedVoucherDto>(grv);
                    return new ServiceResponse<GoodsReceivedVoucherDto>
                    {
                        Data = _grv,
                        IsSuccess = true,
                        Message = $"Goods Received Voucher - {grv.Supplier!.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Goods Received Voucher by id");
                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<GoodsReceivedVoucherDto>> GetByName(string name)
        {
            try
            {
                var grvs = _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(c => c.Supplier!.Name == name)
                    .ToList();

                if (grvs == null)
                {
                    _logger.LogError($"Goods Received Voucher with name: {name} does not exist");
                    return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                    {
                        IsSuccess = false,
                        Message = $"Goods Received Voucher with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);

                    return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_grvs.Count} Goods Received Vouchers",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Goods Received Voucher by name");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<GoodsReceivedVoucherDto>> Update(GoodsReceivedVoucherDto grvDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Map DTO to entity
                var grv = _mapper.Map<GoodsReceivedVoucherDto, GoodsReceivedVoucher>(grvDto);

                // Set GRV status and payment status
                grv.Status = grv.AmountDue == grv.Value
                    ? OrderPaymentStatus.NotPaid
                    : grv.AmountDue > 0
                        ? OrderPaymentStatus.PartiallyPaid
                        : OrderPaymentStatus.Paid;
                grv.IsPaid = grv.AmountDue <= 0;

                // Update received items
                foreach (var item in grv.ReceivedItems!)
                {
                    item.QtyOnHand = item.QtyOnHand; // Likely redundant, can be removed if no modification needed
                    item.IssuedQuantity = 0;
                    item.IsIssued = false;
                }

                _context.GoodsReceivedVouchers!.Update(grv);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    Data = _mapper.Map<GoodsReceivedVoucher, GoodsReceivedVoucherDto>(grv),
                    IsSuccess = true,
                    Message = "Goods Received Voucher Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating grv");
                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    IsSuccess = false,
                    Message = $"Goods Received Voucher Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<GoodsReceivedVoucherDto>> Approve(GoodsReceivedVoucherDto grvDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Map DTO to entity
                var grv = _mapper.Map<GoodsReceivedVoucherDto, GoodsReceivedVoucher>(grvDto);

                // Set GRV status and payment status
                grv.Status = grv.AmountDue == grv.Value
                    ? OrderPaymentStatus.NotPaid
                    : grv.AmountDue > 0
                        ? OrderPaymentStatus.PartiallyPaid
                        : OrderPaymentStatus.Paid;
                grv.IsPaid = grv.AmountDue <= 0;

                // Update received items
                foreach (var item in grv.ReceivedItems!)
                {
                    item.QtyOnHand = item.QtyOnHand; // Likely redundant, can be removed if no modification needed
                    item.IssuedQuantity = 0;
                    item.IsIssued = false;
                }

                _context.GoodsReceivedVouchers!.Update(grv);
                await _context.SaveChangesAsync();

                if (grv.IsApproved)
                {
                    // Batch fetch ProductInventory items for update
                    var inventoryIds = grv.ReceivedItems.Select(item => item.ProductInventoryId).ToList();
                    var inventories = await _context.ProductInventories!
                        .Where(x => inventoryIds.Contains(x.Id))
                        .ToListAsync();

                    foreach (var item in grv.ReceivedItems)
                    {
                        var inventory = inventories.FirstOrDefault(x => x.Id == item.ProductInventoryId);
                        if (inventory != null)
                        {
                            inventory.QuantityOnHand += item.ReceivedQuantity;
                            inventory.Status = inventory.QuantityOnHand > 0 ? Status.InStock : Status.OutOfStock;
                        }
                    }

                    _context.ProductInventories.UpdateRange(inventories);
                    await _context.SaveChangesAsync();

                    // Fetch Purchase Order
                    var order = _context.PurchaceOrders
                        .Include(x => x.PurchaceOrderItems)
                        .FirstOrDefault(x => x.Id == grv.OrderNumber);

                    if (order != null)
                    {
                        // Fetch related GRVs and calculate received quantity
                        var grvs = _context.GoodsReceivedVouchers
                            .Where(x => x.OrderNumber == grv.OrderNumber)
                            .Include(x => x.ReceivedItems)
                            .ToList();

                        var rcvdQty = grvs.Sum(g => g.ReceivedItems.Sum(i => i.ReceivedQuantity));
                        var orderedQty = order.PurchaceOrderItems.Sum(x => x.Quantity);

                        order.IsOpen = rcvdQty < orderedQty ? true : rcvdQty == orderedQty ? false : false;
                        order.IsReceived = true;

                        _context.PurchaceOrders.Update(order);
                    }

                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    Data = _mapper.Map<GoodsReceivedVoucher, GoodsReceivedVoucherDto>(grv),
                    IsSuccess = true,
                    Message = "Approved",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating grv");
                return new ServiceResponse<GoodsReceivedVoucherDto>
                {
                    IsSuccess = false,
                    Message = $"Approval Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllToday()
        {
            try
            {
                var grvs = await _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date == DateTime.Today && x.IsDeleted == false)
                    .ToListAsync();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByDate(DateTime date)
        {
            try
            {
                var grvs = await _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date == date.Date && x.IsDeleted == false)
                    .ToListAsync();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByDateRange(DateTime start, DateTime end)
        {
            try
            {
                var grvs = await _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Date > start.Date && x.CreationTime.Date < end.Date.AddDays(1) && x.IsDeleted == false)
                    .ToListAsync();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllBySupplier(int supplierId)
        {
            try
            {
                var grvs = await _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.SupplierId == supplierId && x.IsDeleted == false)
                    .ToListAsync();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public async Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByMonth(int month)
        {
            try
            {
                var year = DateTime.Now.Year;
                var grvs = await _context.GoodsReceivedVouchers!
                    .Include(x => x.Supplier)
                    .Include(g => g.PurchaceOrderPayments)
                    .Include(x => x.ReceivedItems)!
                        .ThenInclude(x => x.Product)
                    .Where(x => x.CreationTime.Month == month && x.CreationTime.Year == year && x.IsDeleted == false)
                    .ToListAsync();

                var _grvs = _mapper.Map<List<GoodsReceivedVoucher>, List<GoodsReceivedVoucherDto>>(grvs);


                _grvs.ForEach(grv =>
                {
                    grv.PurchaceOrderPayments!.ForEach(pay =>
                    {
                        pay.PaidBy = _context.SystemUsers!.FirstOrDefault(x => x.Id == pay.CreatorId)!.FullName ?? null;
                    });
                });

                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    Data = _grvs,
                    IsSuccess = true,
                    Message = $"Found {_grvs.Count} Goods Received Vouchers",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all grvs");
                return new ServiceResponse<List<GoodsReceivedVoucherDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

       
    }
}
