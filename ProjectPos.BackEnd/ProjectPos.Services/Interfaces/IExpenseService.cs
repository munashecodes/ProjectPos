using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IExpenseService
    {
        public Task<ServiceResponse<ExpenseDto>> CreateAsync(ExpenseDto item);
        public Task<ServiceResponse<ExpenseDto>> ApproveAsync(ExpenseDto item);
        public Task<ServiceResponse<ExpenseDto>> UpdateAsync(ExpenseDto item);
        public Task<ServiceResponse<ExpenseDto>> DeleteAsync(int id, int userId);
        public Task<ServiceResponse<ExpenseDto>> GetByIdAsync(int id);
        public Task<ServiceResponse<List<ExpenseDto>>> GetByAccountAsync(int accId);
        public Task<ServiceResponse<List<ExpenseDto>>> GetByCompanyAsync(int id);
        public Task<ServiceResponse<List<ExpenseDto>>> GetAllAsync();
    }
}
