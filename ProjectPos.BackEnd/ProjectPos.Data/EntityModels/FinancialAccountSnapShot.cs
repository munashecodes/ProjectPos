using System.ComponentModel.DataAnnotations.Schema;
using ProjectPos.Data.AggregateRoots;

namespace ProjectPos.Data.EntityModels;

public class FinancialAccountSnapShot : BasicAggregateRoot<int>
{
    public int? FinancialAccountId { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal? ClosingBalance { get; set; }
    public DateTime SnapShotDate { get; set; }

    [ForeignKey("FinancialAccountId")]
    public Account? FinancialAccount { get; set; }
}