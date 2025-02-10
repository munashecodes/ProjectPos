using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System.Text.Json.Serialization;

namespace ProjectPos.Data.EntityModels
{
    public class AccessLog : BasicAggregateRoot<int>
    {
        public int? UserId { get; set; }
        public DateTime? LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public bool? IsLoggedIn { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SystemName? System { get; set; }
        public string? ComputerName { get; set; }
        public string? ComputerUserName { get; set; }
        public string? ComputerIpAddress { get; set; }
        public string? JwtToken { get; set; }

        public User? User { get; set; }
    }
}
