using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Period
    {
        January = 7,
        February = 8,
        March = 9,
        April = 10,
        May = 11,
        June = 12,
        July = 1,
        August = 2,
        September = 3,
        October = 4,
        November = 5,
        December = 6,
    }
}
