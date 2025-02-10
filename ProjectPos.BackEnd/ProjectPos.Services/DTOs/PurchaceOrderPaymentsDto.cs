﻿using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class PurchaceOrderPaymentsDto : FullAuditedAggregateRoot<int>
    {
        public int? GoodsReceivedVoucherId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? OrderAmount { get; set; }
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
        public Currency? Currency { get; set; }
        public SaleType? MethodOfPay { get; set; }
        public DateTime OrderDate { get; set; }
        public string? SupplierName { get; set; }
        public string? PaidBy { get; set; }
    }
}
