using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class InventoryLevelsReportDto
    {
        public int? ProductId { get; set; }
        public string? BarCode { get; set; }
        public string? Name { get; set; }
        public Grade? Grade { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? OpeningQuantity { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityReceived { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantitySold { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityMovedIn { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityMovedOut { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityDamaged{ get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? QuantityOnHand { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? StockCount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Variance { get; set; }
        public Unit? Unit { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("ProductId")]
        public ProductDto? Product { get; set; }
    }
}
