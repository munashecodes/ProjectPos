namespace ProjectPos.Services.DTOs;

// COGS Report DTOs
public class CogsReportDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalCogs { get; set; }
    public decimal TotalProfit { get; set; }
    public List<CogsReportItemDto> Items { get; set; } = new();
}

public class CogsReportItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal QuantitySold { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal CostPerUnit { get; set; }
    public decimal Revenue => QuantitySold * UnitPrice;
    public decimal Cogs => QuantitySold * CostPerUnit;
    public decimal Profit => Revenue - Cogs;
}

// Cost of Goods Report DTOs
public class CostOfGoodsReportDto
{
    public List<CostOfGoodsReportItemDto> Items { get; set; } = new();
    public decimal TotalEstimatedProfit { get; set; }
}

public class CostOfGoodsReportItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    
    public int SubCategoryId { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal AverageCost { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal EstimatedProfit => (CurrentPrice - AverageCost) * QuantityOnHand;
}