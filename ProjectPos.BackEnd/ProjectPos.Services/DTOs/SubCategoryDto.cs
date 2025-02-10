using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Services.DTOs
{
    public class SubCategoryDto : EntityDto<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
    }
}