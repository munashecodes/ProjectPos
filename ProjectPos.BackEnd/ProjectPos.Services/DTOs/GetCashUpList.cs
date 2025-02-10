using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class GetCashUpList
    {
        public DateTime? CashUpDate { get; set; }
        public int? UserId { get; set; }
        public string? CashUpName { get; set; }
        public List<CashUpDto>? CashUps { get; set; }
    }
}
