using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class ProductInventoryDto : FullAuditedEntityDto<int>
    {
        public string? BarCode { get; set; }
        public string? SubCategoryName { get; set; }
        public string? Name { get; set; }
        public int Flag { get; set; } = 20;
        public string? PLUCode { get; set; }
        public bool? IsWeighted { get; set; } = false;
        public bool? IsTaxable { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        public Grade? Grade { get; set; }
        public decimal? IdealQuantity { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityOnHand { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityOnShelf { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? StockCount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Quantity { get; set; }
        public Unit? Unit { get; set; }
        public Status? Status { get; set; }
        public decimal? MarkUp { get; set; }

        public ProductPriceDto? ProductPrice { get; set; }
    }
}
