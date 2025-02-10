using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ServiceResponse<AccountDto>> CreateAsync(AccountDto item);
        public Task<ServiceResponse<AccountDto>> UpdateAsync(AccountDto item);
        public Task<ServiceResponse<AccountDto>> DeleteAsync(int id, int userId);
        public Task<ServiceResponse<AccountDto>> GetByIdAsync(int id);
        public Task<ServiceResponse<List<AccountDto>>> GetByCategoryAsync(int catId);
        public Task<ServiceResponse<List<AccountDto>>> GetByTypeAsync(AccountType type);
        public Task<ServiceResponse<List<AccountDto>>> GetAllAsync();
    }
}
