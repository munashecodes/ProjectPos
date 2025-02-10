﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountType
    {
        Assets = 1,
        Liability,
        Equity,
        Revenue,
        Expense,
        Income,
    }
}
