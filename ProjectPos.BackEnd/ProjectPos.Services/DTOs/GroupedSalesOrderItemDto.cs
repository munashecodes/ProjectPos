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
    public class GroupedSalesOrderItemDto : EntityDto<int>
    {
        public Category? Category { get; set; }
        public string? SubCategory { get; set; }
        public int? SubCategoryId { get; set; }
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
    }
}
