using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class StockValueReportDto
    {
        public int ProductId { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string SubCategory { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ReceivedStock { get; set; }
        public decimal DamagedStock { get; set; }
        public decimal ReturnedStock { get; set; }
        public decimal SoldStock { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal StockOnHand { get; set; }
        public decimal StockOnShelf { get; set; }
        public decimal Variance { get; set; }
    }
}
