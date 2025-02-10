using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectPos.Services.EntityDtos;

namespace ProjectPos.Services.DTOs
{
    public class ExpenseDto : FullAuditedEntityDto<int>
    {
        [StringLength(500)]
        public string? Description { get; set; } // Optional note for the expense
        public string? ReceiptAttachmentPath { get; set; } // Path to an uploaded receipt or invoice
        [Required]
        public int? AccountId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        [Required]
        [StringLength(255)]
        public string? Payee { get; set; } // Person or Company receiving the payment
        public int? CompanyId { get; set; } // Company that the expense is for
        [Required]
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Amount { get; set; }
        public bool? IsApproved { get; set; } // Whether the expense has been approved
        public int? ApprovedById { get; set; } // User who approved the expense
        public string? CompanyName { get; set; }
        public string? AccountName { get; set; }

        [ForeignKey("AccountId")]
        public AccountDto? Account { get; set; }
        [ForeignKey("CompanyId")]
        public CompanyDto? Company { get; set; }
    }
}