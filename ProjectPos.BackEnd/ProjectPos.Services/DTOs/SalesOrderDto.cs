using ProjectPos.Data.EntityModels;
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
    public class SalesOrderDto : FullAuditedEntityDto<int>
    {
        public SaleType? SaleType { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PriceIncludingVat { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Vat { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Balance { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? PaidAmount { get; set; }
        public Currency? Currency { get; set; }
        public SalesOrderStatus? Status { get; set; }
        public bool? IsPaid { get; set; }
        public string? UserName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        //dependent entities
        public ICollection<SalesOrderItemDto>? SalesOrderItems { get; set; }
        public ICollection<PaymentDto>? Payments { get; set; }

        public CompanyDto? Customer {  get; set; }
    }
}
