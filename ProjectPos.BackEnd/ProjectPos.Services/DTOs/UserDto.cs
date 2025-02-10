using ProjectPos.Services.EntityDtos;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProjectPos.Data.EntityModels;

namespace ProjectPos.Services.DTOs
{
    public class UserDto : EntityDto<int>
    {
        public int? EmployeeId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public Role? Role { get; set; }
        public string? JwtToken { get; set; }
        public bool IsActive { get; set; }
        public string? SupervisorCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SystemName? System { get; set; }
        public ICollection<AccessLogDto>? AccessLogs { get; set; }

        public EmployeeDto? Employee { get; set; }
    }

    public class UserSignInDto : EntityDto<int>
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role? Role { get; set; }
        public string? SupervisorCode { get; set; }
    }
}
