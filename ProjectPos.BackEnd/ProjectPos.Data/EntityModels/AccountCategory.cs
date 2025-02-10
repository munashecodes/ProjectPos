using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class AccountCategory : AuditedAggregateRoot<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AccountType? AccountType { get; set; }

        public List<Account>? Accounts { get; set; }
    }
}
