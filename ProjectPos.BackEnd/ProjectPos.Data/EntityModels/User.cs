using ProjectPos.Data.AggregateRoots;
using ProjectPos.Data.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class User : BasicAggregateRoot<int>
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PasswordHash { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role? Role { get; set; }
        public string? JwtToken { get; set; }
        public bool IsActive { get; set; } = false;
        public string? SupervisorCodeHash { get; set; }

        public ICollection<AccessLog>? AccessLogs { get; set; }
        public ICollection<CashUp>? CashUps { get; set; }

        public Employee? Employee { get; set; }

    }
}
