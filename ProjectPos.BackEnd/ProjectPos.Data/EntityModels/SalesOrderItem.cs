using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPos.Data.EntityModels
{
    public class SalesOrderItem : BasicAggregateRoot<int>
    {
        public int? OrderNumber { get; set; }
        public string? ProductName { get; set; }
        public string? BarCode { get; set; }
        public int? ProductId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Quantity { get; set; }
        public Unit? Unit { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
        public bool isReturned { get; set; } =  false;
        public bool IsPostedToJournal { get; set; } = false;

        [ForeignKey("ProductId")]
        public ProductInventory? Product { get; set; }

        [ForeignKey("OrderNumber")]
        public SalesOrder? SalesOrder { get; set; }

    }
}