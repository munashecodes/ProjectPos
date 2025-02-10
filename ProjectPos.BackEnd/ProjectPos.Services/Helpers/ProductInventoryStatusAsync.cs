using System.Data;
using Coravel.Invocable;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.Helpers;

public class ProductInventoryStatusAsync(IProductInventoryService _inventoryService) : IInvocable
{
    public async Task Invoke()
    {
        await _inventoryService.UpdateProductInventoryStatus();
    }
}