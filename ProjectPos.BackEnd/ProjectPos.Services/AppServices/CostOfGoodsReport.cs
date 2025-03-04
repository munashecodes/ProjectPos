using Microsoft.EntityFrameworkCore;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class CostOfGoodsReport : ICostOfGoodsReport
{
    private readonly ProjectPosDbContext _context;

    public CostOfGoodsReport(ProjectPosDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<CogsReportDto>> GetCogsReport(DateOnly? startDate, DateOnly? endDate)
    {
        var salesItems = await _context.SalesOrderItems
            .Include(s => s.SalesOrder)
            .Include(s => s.Product)
            .Where(s => DateOnly.FromDateTime(s.SalesOrder.CreationTime) >= startDate && DateOnly.FromDateTime(s.SalesOrder.CreationTime) <= endDate)
            .ToListAsync();

        var report = new CogsReportDto();

        foreach (var item in salesItems)
        {
            var productId = item.ProductId ?? 0;
            var costPerUnit = await GetProductCost(item.ProductId.Value, item.SalesOrder.CreationTime);

            report.Items.Add(new CogsReportItemDto
            {
                ProductId = productId,
                ProductName = item.Product?.Name ?? "N/A",
                QuantitySold = item.Quantity ?? 0,
                UnitPrice = item.UnitPrice ?? 0,
                CostPerUnit = costPerUnit
            });
        }

        report.TotalRevenue = report.Items.Sum(i => i.Revenue);
        report.TotalCogs = report.Items.Sum(i => i.Cogs);
        report.TotalProfit = report.TotalRevenue - report.TotalCogs;

        return new ServiceResponse<CogsReportDto>
        {
            Data = report,
            IsSuccess = true,
            Message = "Success",
            Time = DateTime.Now

        };
    }

    public async Task<ServiceResponse<CostOfGoodsReportDto>> GetCostOfGoodsReport()
    {
        var products = await _context.ProductInventories
            .Include(p => p.ReceivedItems)
            .Include(p => p.ProductPrices)
            .ToListAsync();

        var report = new CostOfGoodsReportDto();

        foreach (var product in products)
        {
            var (averageCost, currentPrice) = await GetProductCostAndPrice(product.Id);

            report.Items.Add(new CostOfGoodsReportItemDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                SubCategoryId = (int)product.SubCategoryId,
                QuantityOnHand = product.QuantityOnHand ?? 0,
                AverageCost = averageCost,
                CurrentPrice = currentPrice
            });
        }

        report.TotalEstimatedProfit = report.Items.Sum(i => i.EstimatedProfit);
        return new ServiceResponse<CostOfGoodsReportDto>
        {
            Data = report,
            IsSuccess = true,
            Message = "Success",
            Time = DateTime.Now

        };
    }

    private async Task<decimal> GetProductCost(int productId, DateTime? beforeDate = null)
    {
        var grvQuery = _context.GoodsReceivedVoucherLines
            .Where(g => g.ProductInventoryId == productId);

        if (beforeDate.HasValue)
            grvQuery = grvQuery.Where(g => g.ReceivedDate <= beforeDate);

        var grvItems = await grvQuery.ToListAsync();

        if (grvItems.Any())
        {
            var totalQuantity = grvItems.Sum(g => g.ReceivedQuantity ?? 0);
            var totalCost = grvItems.Sum(g => (decimal)(g.UnitPrice ?? 0) * (g.ReceivedQuantity ?? 0));
            return totalQuantity > 0 ? totalCost / totalQuantity : 0;
        }

        var latestPrice = await _context.ProductPrices
            .Where(p => p.ProductInventoryId == productId)
            .OrderByDescending(p => p.CreationTime)
            .FirstOrDefaultAsync();

        return latestPrice?.Cost ?? 0;
    }

    private async Task<(decimal averageCost, decimal currentPrice)> GetProductCostAndPrice(int productId)
    {
        var averageCost = await GetProductCost(productId);
        
        var currentPrice = await _context.ProductPrices
            .Where(p => p.ProductInventoryId == productId)
            .OrderByDescending(p => p.CreationTime)
            .Select(p => p.Price)
            .FirstOrDefaultAsync();

        return (averageCost, (decimal)currentPrice);
    }
}