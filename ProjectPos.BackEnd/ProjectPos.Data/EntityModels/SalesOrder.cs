using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Data.EntityModels
{
    public class SalesOrder : FullAuditedAggregateRoot<int>
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
        public Currency? Currency { get; set; }
        public SalesOrderStatus? Status { get; set; }
        public bool? IsPaid { get; set; }
        public int? CustomerId { get; set;}
        public bool IsPostedToJournal { get; set; } = false;

        //foreign keys
        [ForeignKey("CustomerId")]
        public Company? Customer { get; set; }

        //dependent entities
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<SalesOrderItem>? SalesOrderItems { get; set; }
    }
}   