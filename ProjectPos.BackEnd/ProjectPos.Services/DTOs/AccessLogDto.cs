using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.DTOs
{
    public class AccessLogDto : EntityDto<int>
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public bool? IsLoggedIn { get; set; }
        public SystemName? System { get; set; }
        public string? ComputerName { get; set; }
        public string? ComputerUserName { get; set; }
        public string? ComputerIpAddress { get; set; }
        public string? JwtToken { get; set; }
    }
}
