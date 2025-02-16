using System.Text.Json.Serialization;
using ProjectPos.Data.Shared.Enums;

namespace ProjectPos.Services.DTOs;

public class PayRollCycleDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsClosed { get; set; } = false;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PayRollStatus PayRollStatus { get; set; } = PayRollStatus.Pending;

    public List<PaySlipDto>? PaySlips { get; set; }
}