using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Services.DTOs
{
    public class GetSalesOrderByUserDto
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<SalesOrderDto>? SalesOrders { get; set; }
        public decimal? AmountTotal { get; set; }
        public decimal? PaidTotal { get; set; }
        
        public SaleType? MethodOfPay{ get; set; }
        public string? EcocashSuccessCode { get; set; }
        public decimal? BalanceTotal { get; set; }
    }
}
