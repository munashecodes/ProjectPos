using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class ReconDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Rate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? USDAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? SalesAmount { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Variance { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set; }
        public DateTime? CashUpDate { get; set; }
    }
}
