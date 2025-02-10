using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.ReportingDtos
{
    public class SalesEndDayReportDto
    {
        public string? UserName { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? SalesTotal { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? CashTotal { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? Variance { get; set; }
        public DateTime? Date { get; set; }
    }
}
