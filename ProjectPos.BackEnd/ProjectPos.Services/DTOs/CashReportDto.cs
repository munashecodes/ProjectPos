using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class CashReportDto
    {
        public decimal? UsdAmount { get; set; }
        public string Currency { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? InHand { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Varience { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Amount { get; set; }
    }
}
