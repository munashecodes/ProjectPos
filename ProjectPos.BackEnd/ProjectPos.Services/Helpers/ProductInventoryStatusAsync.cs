using System.Data;
using Coravel.Invocable;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.Helpers;

public class ProductInventoryStatusAsync(IProductInventoryService _inventoryService, INotificationService _notificationService) : IInvocable
{
    public async Task Invoke()
    {
        await _inventoryService.UpdateProductInventoryStatus();
        await _notificationService.GetPurchaseOrderPending();
    }
}