﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Grade
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        None = 6,
    }
}
