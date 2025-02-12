using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface INotificationService
{
    public Task<ServiceResponse<NotificationDto>> GetPurchaseOrderPending();
}