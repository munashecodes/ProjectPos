namespace ProjectPos.Services.DTOs;

public class IncomeStatementDto
{
    public decimal TotalSales { get; set; }
    public decimal CostOfGoodsSold { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal OperatingExpenses { get; set; }
    public decimal OperatingProfit { get; set; }
    public decimal Taxes { get; set; }
    public decimal NetProfit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Detailed breakdowns
    public Dictionary<string, decimal> SalesBreakdown { get; set; } = new();
    public Dictionary<string, decimal> ExpenseBreakdown { get; set; } = new();
    public Dictionary<string, decimal> TaxBreakdown { get; set; } = new();
}