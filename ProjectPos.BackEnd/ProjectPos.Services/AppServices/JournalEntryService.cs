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
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class JournalEntryService : IJournalEntryService
    {
        //inject dbcontext and IMapper here
        private readonly IMapper _mapper;
        private readonly ProjectPosDbContext _context;
        private ILogger<JournalEntryService> _logger;

        public JournalEntryService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<JournalEntryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<JournalEntryDto>> CreateAsync(JournalEntryDto item)
{
    try
    {
        var journalEntry = _mapper.Map<JournalEntry>(item);
        //get the accounts from the db
        journalEntry.JournalEntryLines!.ForEach(async jel =>
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == jel.AccountId);
            if (account == null)
                throw new Exception($"Account with ID {jel.AccountId} not found.");

            // Adjust balance based on account type and journal entry type
            switch (account.AccountType)
            {
                case AccountType.Assets:
                case AccountType.Expense:
                    account.Balance = (decimal)(jel.Type == JournalEntryType.Debit
                        ? account.Balance + jel.Amount
                        : account.Balance - jel.Amount);
                    break;

                case AccountType.Liability:
                case AccountType.Equity:
                case AccountType.Revenue:
                case AccountType.Income:
                    account.Balance = (decimal)(jel.Type == JournalEntryType.Credit
                        ? account.Balance + jel.Amount
                        : account.Balance - jel.Amount);
                    break;

                default:
                    throw new Exception($"Unhandled account type: {account.AccountType}");
            }

            //update account balance
            _context.Accounts.Update(account);

        });

        await _context.JournalEntries.AddAsync(journalEntry);
        await _context.SaveChangesAsync();

        return new ServiceResponse<JournalEntryDto>
        {
            IsSuccess = true,
            Data = _mapper.Map<JournalEntryDto>(journalEntry),
            Message = "Journal Entry created successfully",
            Time = DateTime.UtcNow
        };
    }
    catch (Exception ex)
    {
        //log error here
        _logger.LogError(ex, ex.Message);
        return new ServiceResponse<JournalEntryDto>
        {
            IsSuccess = false,
            Message = ex.Message
        };
    }
}

        public Task<ServiceResponse<JournalEntryDto>> DeleteAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<JournalEntryDto>>> GetAllAsync()
        {
            try
            {
                var journalEntries = await _context.JournalEntries
                    .Include(je => je.JournalEntryLines)!
                        .ThenInclude(jel => jel.Account)
                    .ToListAsync();

                return new ServiceResponse<List<JournalEntryDto>>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<List<JournalEntryDto>>(journalEntries),
                    Message = "Journal Entries retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<JournalEntryDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<List<JournalEntryDto>>> GetByCategoryAsync(int catId)
        {
            try
            {
                var journalEntries = await _context.JournalEntries
                    .Include(je => je.JournalEntryLines)!
                        .ThenInclude(jel => jel.Account)
                    .Where(je => je.JournalEntryLines.Any(jel => jel.Account.AccountCategoryId == catId))
                    .ToListAsync();

                return new ServiceResponse<List<JournalEntryDto>>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<List<JournalEntryDto>>(journalEntries),
                    Message = "Journal Entries retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<JournalEntryDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<JournalEntryDto>> GetByIdAsync(int id)
        {
            try
            {
                var journalEntries = await _context.JournalEntries
                    .Include(je => je.JournalEntryLines)!
                        .ThenInclude(jel => jel.Account)
                    .FirstOrDefaultAsync(je => je.Id == id);

                return new ServiceResponse<JournalEntryDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<JournalEntryDto>(journalEntries),
                    Message = "Journal Entries retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<JournalEntryDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public Task<ServiceResponse<List<JournalEntryDto>>> GetByTypeAsync(AccountType type)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<JournalEntryDto>> UpdateAsync(JournalEntryDto item)
        {
            throw new NotImplementedException();
        }
    }
}
