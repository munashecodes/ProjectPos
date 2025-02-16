using System.Text.Json.Serialization;

namespace ProjectPos.Data.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Department
{
    Admin = 1,
    Sales,
    Finance,
    Warehouse,
    Production,
    HR,
    IT,
    Marketing,
    Procurement,
    QualityAssurance,
    ResearchAndDevelopment,
    CustomerService,
    Legal,
    Management,
    Other
}

