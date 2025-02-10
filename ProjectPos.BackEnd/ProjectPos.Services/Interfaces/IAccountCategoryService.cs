using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IAccountCategoryService
    {
        public Task<ServiceResponse<AccountCategoryDto>> CreateAsync(AccountCategoryDto item);
        public Task<ServiceResponse<AccountCategoryDto>> UpdateAsync(AccountCategoryDto item);
        public Task<ServiceResponse<AccountCategoryDto>> DeleteAsync(int id, int userId);
        public Task<ServiceResponse<AccountCategoryDto>> GetByIdAsync(int id);
        public Task<ServiceResponse<List<AccountCategoryDto>>> GetByNameAsync(string name);
        public Task<ServiceResponse<List<AccountCategoryDto>>> GetAllAsync();
    }
}
