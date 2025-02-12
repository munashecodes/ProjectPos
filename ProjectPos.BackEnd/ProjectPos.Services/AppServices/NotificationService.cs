using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class NotificationService(ProjectPosDbContext _context, ILogger<NotificationService> _logger) : INotificationService
{
    public async Task<ServiceResponse<NotificationDto>> GetPurchaseOrderPending()
    {
        try
        {
            var pendingOrders = await _context.PurchaceOrders.Where(po => !po.IsApproved).ToListAsync();
            var pendingReceiving = await _context.GoodsReceivedVouchers.Where(po => !po.IsApproved).ToListAsync();
            var response = new NotificationDto
            {
                PurchaseOrder = pendingOrders.Count,
                ReceivingOrder = pendingReceiving.Count(),

            };
            return new ServiceResponse<NotificationDto>
            {
                Data = response,
                IsSuccess = true,
                Time = DateTime.Now,
                Message = "Success"
            };
        }
        catch (Exception e)
        {
            _logger.LogError("Error getting notifications" + e.Message);
            return new ServiceResponse<NotificationDto>
            {
                IsSuccess = false,
                Message = e.Message,
                Time = DateTime.Now,

            };
        }
        
    }
}