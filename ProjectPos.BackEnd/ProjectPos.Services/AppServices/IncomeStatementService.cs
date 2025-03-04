using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class IncomeStatementService : IIncomeStatementService
{
    private readonly ProjectPosDbContext _context;
    private readonly ILogger<IncomeStatementService> _logger;

    public IncomeStatementService(ProjectPosDbContext context, ILogger<IncomeStatementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ServiceResponse<IncomeStatementDto>> GenerateIncomeStatementAsync(
        DateTime startDate, 
        DateTime endDate)
    {
        try
        {
            var incomeStatement = new IncomeStatementDto
            {
                StartDate = startDate,
                EndDate = endDate
            };

            // Calculate Total Sales from SalesOrders
            var salesData = await CalculateSalesDataAsync(startDate, endDate);
            incomeStatement.TotalSales = salesData.Sum(x => x.Value);
            incomeStatement.SalesBreakdown = salesData;

            // Calculate COGS from GoodsReceivedVouchers and SalesOrderItems
            incomeStatement.CostOfGoodsSold = await CalculateCOGSAsync(startDate, endDate);

            // Calculate Gross Profit
            incomeStatement.GrossProfit = incomeStatement.TotalSales - incomeStatement.CostOfGoodsSold;

            // Calculate Operating Expenses from Expenses table
            var expenseData = await CalculateOperatingExpensesAsync(startDate, endDate);
            incomeStatement.OperatingExpenses = expenseData.Sum(x => x.Value);
            incomeStatement.ExpenseBreakdown = expenseData;

            // Calculate Operating Profit
            incomeStatement.OperatingProfit = incomeStatement.GrossProfit - incomeStatement.OperatingExpenses;

            // Calculate Taxes from PaySlips (PAYE) and SalesOrders (VAT)
            var taxData = await CalculateTaxesAsync(startDate, endDate);
            incomeStatement.Taxes = taxData.Sum(x => x.Value);
            incomeStatement.TaxBreakdown = taxData;

            // Calculate Net Profit
            incomeStatement.NetProfit = incomeStatement.OperatingProfit - incomeStatement.Taxes;

            return ServiceResponse<IncomeStatementDto>.Success(incomeStatement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating income statement for period {StartDate} to {EndDate}", 
                startDate, endDate);
            return ServiceResponse<IncomeStatementDto>.Failure($"Failed to generate income statement: {ex.Message}");
        }
    }

    private async Task<Dictionary<string, decimal>> CalculateSalesDataAsync(DateTime startDate, DateTime endDate)
    {
        var salesBreakdown = new Dictionary<string, decimal>();

        var salesData = await _context.SalesOrders!
            .Where(x => x.CreationTime.Date >= startDate.Date && 
                        x.CreationTime.Date <= endDate.Date)
            .GroupBy(x => x.SaleType)
            .Select(g => new
            {
                SaleType = g.Key,
                Total = g.Sum(x => x.PriceIncludingVat ?? 0m)
            })
            .ToListAsync();

        foreach (var sale in salesData)
        {
            salesBreakdown.Add($"Sales_{sale.SaleType}", sale.Total);
        }

        return salesBreakdown;
    }

    private async Task<decimal> CalculateCOGSAsync(DateTime startDate, DateTime endDate)
    {
        // Get cost of goods sold from SalesOrderItems using product cost
        var totalCogs = 0m;
        var cogs = await _context.SalesOrderItems!
            .Include(x => x.SalesOrder)
            .Include(x => x.Product)
            .Where(x => x.SalesOrder!.CreationTime.Date >= startDate.Date && 
                        x.SalesOrder.CreationTime.Date <= endDate.Date)
            .ToListAsync();

        foreach (var item in cogs)
        {
            var cost = await GetProductCost((int)item.ProductId, item.SalesOrder.CreationTime);
            var totalCost = cost * (decimal)item.Quantity;
            totalCogs += totalCost;
        }

        return totalCogs;
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

    private async Task<Dictionary<string, decimal>> CalculateOperatingExpensesAsync(
        DateTime startDate, 
        DateTime endDate)
    {
        var expenseBreakdown = new Dictionary<string, decimal>();

        // Get expenses grouped by type
        var expenses = await _context.Expenses!
            .Include(x => x.Account)
            .Where(x => x.CreationTime.Date >= startDate.Date && 
                        x.CreationTime.Date <= endDate.Date)
            .GroupBy(x => x.Account!.Name)
            .Select(g => new
            {
                ExpenseType = g.Key,
                Total = g.Sum(x => (decimal?)x.Amount ?? 0)
            })
            .ToListAsync();

        foreach (var expense in expenses)
        {
            expenseBreakdown.Add($"Expense_{expense.ExpenseType}", expense.Total);
        }

        // Add payroll expenses
        var payrollExpense = await _context.PaySlips!
            .Where(x => x.CreationTime.Date >= startDate.Date && 
                        x.CreationTime.Date <= endDate.Date)
            .SumAsync(x => x.GrossSalary ?? 0m);

        expenseBreakdown.Add("Payroll_Expenses", payrollExpense);

        return expenseBreakdown;
    }

    private async Task<Dictionary<string, decimal>> CalculateTaxesAsync(DateTime startDate, DateTime endDate)
    {
        var taxBreakdown = new Dictionary<string, decimal>();

        // Calculate VAT from sales
        var vat = await _context.SalesOrders!
            .Where(x => x.CreationTime.Date >= startDate.Date && 
                        x.CreationTime.Date <= endDate.Date)
            .SumAsync(x => x.Vat ?? 0m);

        taxBreakdown.Add("VAT", vat);

        // Calculate PAYE from payslips
        var paye = await _context.PaySlips!
            .Where(x => x.CreationTime.Date >= startDate.Date && 
                        x.CreationTime.Date <= endDate.Date)
            .SumAsync(x => x.Tax ?? 0m);

        taxBreakdown.Add("PAYE", paye);

        return taxBreakdown;
    }
}