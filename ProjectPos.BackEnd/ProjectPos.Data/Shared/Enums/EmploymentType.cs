using System.Text.Json.Serialization;

namespace ProjectPos.Data.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentType
{
    FullTime = 1,
    PartTime,
    Contract,
    Internship
}