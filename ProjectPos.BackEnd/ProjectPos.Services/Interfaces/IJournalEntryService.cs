using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IJournalEntryService
    {
        public Task<ServiceResponse<JournalEntryDto>> CreateAsync(JournalEntryDto item);
        public Task<ServiceResponse<JournalEntryDto>> UpdateAsync(JournalEntryDto item);
        public Task<ServiceResponse<JournalEntryDto>> DeleteAsync(int id, int userId);
        public Task<ServiceResponse<JournalEntryDto>> GetByIdAsync(int id);
        public Task<ServiceResponse<List<JournalEntryDto>>> GetByCategoryAsync(int catId);
        public Task<ServiceResponse<List<JournalEntryDto>>> GetByTypeAsync(AccountType type);
        public Task<ServiceResponse<List<JournalEntryDto>>> GetAllAsync();
    }
}
