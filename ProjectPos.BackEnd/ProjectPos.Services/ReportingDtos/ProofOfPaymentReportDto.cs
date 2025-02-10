using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.ReportingDtos
{
    public class ProofOfPaymentReportDto
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? BookingDate { get; set; }
        public IEnumerable<ProofOfPaymentDto>? ProofOfPayments { get; set; }
        public decimal? PaidTotal { get; set; }
        public decimal? UsableTotal { get; set; }
        public decimal? UsedTotal { get; set; }
    }
}
