using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPos.Data.EntityModels;

namespace ProjectPos.Services.DTOs
{
    public class ProductDto : FullAuditedEntityDto<int>
    {
        public string? BarCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Cost { get; set; }
        public Grade? Grade { get; set; }
        public bool? IsTaxable { get; set; }
    }
}
