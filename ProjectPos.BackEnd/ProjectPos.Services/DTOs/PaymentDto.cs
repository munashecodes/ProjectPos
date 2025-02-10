using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class PaymentDto : FullAuditedEntityDto<int>
    {
        public int? SalesOrderId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? TotalPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? USDPaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal PaidAmountAfterChange { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal USDPaidAmountAfterChange { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ChangeAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ExchangeRate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? VAT { get; set; }
        public Currency? Currency { get; set; }
        public SaleType? MethodOfPay { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
