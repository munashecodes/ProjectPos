using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class Payment : FullAuditedAggregateRoot<int>
    {
        public int? SalesOrderId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? TotalPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? USDPaidAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal PaidAmountAfterChange { get; set; } = 0;
        [Column(TypeName = "decimal(12, 2)")]
        public decimal USDPaidAmountAfterChange { get; set; } = 0;
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ChangeAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ExchangeRate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? VAT { get; set; }
        public Currency? Currency { get; set; }
        public SaleType? MethodOfPay { get; set; }
        public DateTime OrderDate { get; set; }
        public int? ProofOfPaymentId { get; set; }
        public bool IsPostedToJournal { get; set; } = false;

        //foreign keys
        [ForeignKey("SalesOrderId")]
        public SalesOrder? SalesOrder { get;}
    }
}
