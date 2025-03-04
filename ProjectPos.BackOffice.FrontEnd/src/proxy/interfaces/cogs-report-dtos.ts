
export interface CogsReport {
    totalRevenue: number;
    totalCogs: number;
    totalProfit: number;
    items: CogsReportItem[];
  }
  
  export interface CogsReportItem {
    productId: number;
    productName: string;
    quantitySold: number;
    unitPrice: number;
    costPerUnit: number;
    revenue: number; // Derived from quantitySold * unitPrice
    cogs: number; // Derived from quantitySold * costPerUnit
    profit: number; // Derived from revenue - cogs
  }
  
  // Cost of Goods Report Interfaces
  export interface CostOfGoodsReport {
    items: CostOfGoodsReportItem[];
    totalEstimatedProfit: number;
  }
  
  export interface CostOfGoodsReportItem {
    productId: number;
    productName: string;
    quantityOnHand: number;
    subCategoryId: number;
    averageCost: number;
    currentPrice: number;
    estimatedProfit: number; // Derived from (currentPrice - averageCost) * quantityOnHand
  }
  
