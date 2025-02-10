using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class StockTakeReportDto
    {
        public int ProductId { get; set; }
        public string? BarCode { get; set; }
        public string? ProductName { get; set; }
        public string? SubCategory { get; set; }
        public Unit? Unit { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal DamagedQuantity { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal SoldQuantity { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal QuantityOnShelf { get; set; }
        public decimal Variance { get; set; }
    }
}
