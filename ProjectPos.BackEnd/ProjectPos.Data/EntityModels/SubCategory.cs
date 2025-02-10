using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Data.EntityModels
{
    public class SubCategory : BasicAggregateRoot<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }

        public ICollection<ProductInventory>? Products { get; set; }
    }
}