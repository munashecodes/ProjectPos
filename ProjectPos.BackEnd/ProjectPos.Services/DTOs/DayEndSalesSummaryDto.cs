using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class DayEndSalesSummaryDto : FullAuditedEntityDto<int>
    {
        // Sales Summary
        public decimal TotalSales { get; set; } = 0;
        public int TotalTransactions { get; set; } = 0;
        public decimal AverageTransactionValue { get; set; } = 0;

        // Payment Breakdown
        public decimal CashSales { get; set; } = 0;
        public decimal CashSalesReported { get; set; } = 0;
        public decimal CardSales { get; set; } = 0;
        public decimal CreditSales { get; set; } = 0;
        public decimal MobilePayments { get; set; } = 0;

        // Discounts and Refunds
        public decimal TotalDiscounts { get; set; } = 0;
        public int TotalReturns { get; set; } = 0;
        public decimal ReturnAmount { get; set; } = 0;

        // Voided and Cancelled Transactions
        public int VoidedTransactions { get; set; } = 0;
        public decimal VoidedAmount { get; set; } = 0;
        public int CancelledTransactions { get; set; } = 0;
    }
}
