using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class ProductInventory : FullAuditedAggregateRoot<int>
    {
        public string? BarCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Flag { get; set; } = 0;
        public string? PLUCode { get; set; }
        public Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public bool? IsWeighted { get; set; } = false;
        public bool? IsTaxable { get; set; } = false;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Grade? Grade { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? IdealQuantity { get; set; } = 0;

        [Column(TypeName = "decimal(12, 2)")] 
        public decimal? QuantityOnHand { get; set; } = 0;

        [Column(TypeName = "decimal(12, 2)")] 
        public decimal? QuantityOnShelf { get; set; } = 0;
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? StockCount { get; set; } = 0;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Unit? Unit { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status? Status { get; set; } = Shared.Enums.Status.OutOfStock;
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? MarkUp { get; set; }

        public ICollection<ProductPrice>? ProductPrices { get; set; }
        public List<PurchaceOrderLine>? OrderedItems { get; set; }
        public ICollection<GoodsReceivedVoucherLine>? ReceivedItems { get; set; }
        public ICollection<StockMovement>? StockMovements { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
