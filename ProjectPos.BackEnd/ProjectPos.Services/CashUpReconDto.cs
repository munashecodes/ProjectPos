using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services
{
    public class CashUpReconDto
    {
        public int RecieptNumber { get; set; }
        public string? PaxName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public decimal? ChangeAmount { get; set; }
        public decimal? PaymentTotal { get; set; }
        public decimal? CashUpTotal { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? CashTotal { get; set; }
        public ICollection<CashReportDto>? CashReport { get; set; }
        public ICollection<CreditReportDto>? CreditReport { get; set; }
        public ICollection<EcoCashReportDto>? EcoCashReport { get; set; }
        public ICollection<CreditCardReportDto>? CreditCardReport { get; set; }
    }
}
