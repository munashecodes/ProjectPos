using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;

namespace ProjectPos.Services.DTOs
{
    public class AccountCategoryDto : AuditedEntityDto<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AccountType? AccountType { get; set; }

        public List<AccountDto>? Accounts { get; set; }
    }
}