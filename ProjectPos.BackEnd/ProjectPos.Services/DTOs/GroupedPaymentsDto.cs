using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    internal class GroupedPaymentsDto
    {
        public int? UserId { get; set; }
        public decimal? ChangeAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? InvoiceValue { get; set; }
        public List<PaymentDto>? Payments { get; set; }
    }
}
