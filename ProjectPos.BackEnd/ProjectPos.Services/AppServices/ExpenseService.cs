using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class ExpenseService : IExpenseService
    {
        //inject dbcontext and IMapper here
        private readonly IMapper _mapper;
        private readonly ProjectPosDbContext _context;
        private ILogger<AccountService> _logger;

        public ExpenseService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<ExpenseDto>> CreateAsync(ExpenseDto item)
        {
            try
            {
                var expense = _mapper.Map<Expense>(item);

                expense.PrepareEntityForCreate();

                // Add expense and save changes
                var res = await _context.Expenses.AddAsync(expense);
                await _context.SaveChangesAsync();

                var newExpense = await _context.Expenses.Include(e => e.Company).Include(e => e.Account).FirstOrDefaultAsync(e => e.Id == res.Entity.Id);

                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ExpenseDto>(newExpense),
                    Message = "Expense created successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ExpenseDto>> DeleteAsync(int id, int userId)
        {
            try
            {
                var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
                if (expense == null)
                {
                    //log error here
                    _logger.LogError("Expense not found");
                    return new ServiceResponse<ExpenseDto>
                    {
                        IsSuccess = false,
                        Message = "Expense not found"
                    };
                }

                // Soft delete expense
                expense.IsDeleted = true;
                expense.DeleterId = userId;
                expense.DeletionTime = DateTime.UtcNow;

                _context.Expenses.Update(expense);
                await _context.SaveChangesAsync();

                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ExpenseDto>(expense),
                    Message = "Expense deleted successfully",
                    Time = DateTime.UtcNow
                };

            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetAllAsync()
        {
            try 
            {                 
                var expenses = await _context.Expenses
                    .Include(e => e.Account)
                    .Include(e => e.Creator)
                    .Include(e => e.Company)
                    .Include(e => e.ApprovedBy)
                    .Include(e => e.LastModifierUser)
                    .Where(e =>  e.IsDeleted == false)
                    .ToListAsync();

                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<List<ExpenseDto>>(expenses),
                    Message = "Expenses retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetByAccountAsync(int accId)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Include(e => e.Account)
                    .Include(e => e.Creator)
                    .Include(e => e.Company)
                    .Include(e => e.ApprovedBy)
                    .Include(e => e.LastModifierUser)
                    .Where(e => e.AccountId == accId && e.IsDeleted == false)
                    .ToListAsync();

                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<List<ExpenseDto>>(expenses),
                    Message = "Expenses retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<List<ExpenseDto>>> GetByCompanyAsync(int id)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Include(e => e.Account)
                    .Include(e => e.Creator)
                    .Include(e => e.Company)
                    .Include(e => e.ApprovedBy)
                    .Include(e => e.LastModifierUser)
                    .Where(e => e.CompanyId == id && e.IsDeleted == false)
                    .ToListAsync();

                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<List<ExpenseDto>>(expenses),
                    Message = "Expenses retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<ExpenseDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ExpenseDto>> GetByIdAsync(int id)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Include(e => e.Account)
                    .Include(e => e.Creator)
                    .Include(e => e.Company)
                    .Include(e => e.ApprovedBy)
                    .Include(e => e.LastModifierUser)
                    .Where(e => e.Id == id && e.IsDeleted == false)
                    .ToListAsync();

                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ExpenseDto>(expenses),
                    Message = "Expenses retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ExpenseDto>> UpdateAsync(ExpenseDto item)
        {
            try
            {
                var expense = _mapper.Map<Expense>(item);

                // Add expense and save changes
                var res = _context.Expenses.Update(expense);
                await _context.SaveChangesAsync();

                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ExpenseDto>(expense),
                    Message = "Expense created successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ExpenseDto>> ApproveAsync(ExpenseDto item)
        {
            try
            {
                var expense = _mapper.Map<Expense>(item);

                // Generate a journal entry for the expense
                var journalEntry = new JournalEntry
                {
                    CreationTime = DateTime.UtcNow,
                    Description = $"Created Expense: {expense.Description}",
                    CreatorId = expense.CreatorId,
                    IsDeleted = false,
                    JournalEntryLines = new List<JournalEntryLine>
                    {
                        new JournalEntryLine
                        {
                            AccountId = expense.AccountId,
                            Amount = expense.Amount,
                            Type = JournalEntryType.Debit
                        },
                        new JournalEntryLine
                        {
                            AccountId = expense.PaymentMethod switch
                            {
                                PaymentMethod.EcoCash => 24,
                                PaymentMethod.Cash => 16,
                                _ => 17
                            },
                            Amount = expense.Amount,
                            Type = JournalEntryType.Credit
                        }
                    }
                };

                await _context.JournalEntries.AddAsync(journalEntry);

                // Process account balance updates
                var accountIds = journalEntry.JournalEntryLines.Select(jel => jel.AccountId).Distinct();
                var accounts = await _context.Accounts
                    .Where(a => accountIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var jel in journalEntry.JournalEntryLines)
                {
                    var account = accounts.FirstOrDefault(a => a.Id == jel.AccountId);
                    if (account == null)
                        throw new Exception($"Account with ID {jel.AccountId} not found.");

                    // Adjust balance based on account type and journal entry type
                    account.Balance += AdjustAccountBalance(
                        (AccountType)account.AccountType!,
                        (JournalEntryType)jel.Type!,
                        (decimal)jel.Amount!
                        );
                }

                

                // Update accounts in bulk
                _context.Accounts.UpdateRange(accounts);

                foreach (var account in accounts)
                {
                    var trackedEntity = _context.ChangeTracker.Entries<Account>()
                        .FirstOrDefault(e => e.Entity.Id == account.Id);

                    if (trackedEntity != null)
                    {
                        _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
                    }
                }


                // Add expense and save changes
                _context.Expenses.Update(expense);
                await _context.SaveChangesAsync();

                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<ExpenseDto>(expense),
                    Message = "Expense created successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<ExpenseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        // Helper method to adjust account balance
        static decimal AdjustAccountBalance(AccountType accountType, JournalEntryType entryType, decimal amount)
        {
            return accountType switch
            {
                AccountType.Assets or AccountType.Expense => entryType == JournalEntryType.Debit ? amount : -amount,
                AccountType.Liability or AccountType.Equity or AccountType.Revenue or AccountType.Income => entryType == JournalEntryType.Credit ? amount : -amount,
                _ => throw new Exception($"Unhandled account type: {accountType}")
            };
        }

    }
}
