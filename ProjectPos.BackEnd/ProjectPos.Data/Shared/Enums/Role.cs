using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Admin = 1,
        Supervisor = 2,
        Manager = 3,
        SysAdmin = 4,
        Cashier = 5,
        Accountant = 6,
        AccountsClerk = 7,
        DebtorsController = 8,
        FinanceManager = 9,
        SysDeveloper = 10,
        StockController = 11,
        ReceivingClerk = 12
    }
}
