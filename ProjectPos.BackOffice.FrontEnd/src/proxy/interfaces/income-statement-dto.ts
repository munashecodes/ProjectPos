export interface IncomeStatementDto {
    totalSales: number;
    costOfGoodsSold: number;
    grossProfit: number;
    operatingExpenses: number;
    operatingProfit: number;
    taxes: number;
    netProfit: number;
    startDate: Date;
    endDate: Date;
    
    // Detailed breakdowns
    salesBreakdown: { [key: string]: number };
    expenseBreakdown: { [key: string]: number };
    taxBreakdown: { [key: string]: number };
} 