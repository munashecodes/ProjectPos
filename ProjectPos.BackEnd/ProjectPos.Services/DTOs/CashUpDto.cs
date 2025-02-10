using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class CashUpDto : FullAuditedEntityDto<int>
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency? Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Rate { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
        public decimal? USDAmount { get; set; }
        public string? UserName { get; set; }
    }
}
