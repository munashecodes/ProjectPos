using ProjectPos.Data.EntityModels;
using ProjectPos.Services.EntityDtos;

namespace ProjectPos.Services.DTOs
{
    public class JournalEntryDto : AuditedEntityDto<int>
    {
        public string? Description { get; set; }

        public List<JournalEntryLineDto>? JournalEntryLines { get; set; }
    }
}