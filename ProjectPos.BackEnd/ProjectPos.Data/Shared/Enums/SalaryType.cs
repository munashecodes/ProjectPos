using System.Text.Json.Serialization;

namespace ProjectPos.Data.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SalaryType
{
    Hourly = 1,
    Daily,
    Weekly,
    Monthly
}