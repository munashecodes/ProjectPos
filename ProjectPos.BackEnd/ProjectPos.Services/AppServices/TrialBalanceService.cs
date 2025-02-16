using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Services.AppServices;

public class TrialBalanceService : ITrialBalanceService
{
    private readonly ProjectPosDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TrialBalanceService> _logger;  

    public TrialBalanceService(
        ProjectPosDbContext context,
        IMapper mapper,
        ILogger<TrialBalanceService> logger
        )

    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByDateAsync(DateTime date)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByDateRangeAsync(DateTime start, DateTime end)
    {
        try
        {
            var openingBalances = await _context.FinancialAccountSnapShots
                                        .Where(a => a.SnapShotDate.Date == start.Date.AddDays(-1))
                                        .ToListAsync();

            var lastSnap = await _context.FinancialAccountSnapShots
                                        .OrderByDescending(a => a.SnapShotDate)
                                        .FirstOrDefaultAsync(a => a.SnapShotDate.Date <= end.Date);

            if (lastSnap == null)
            {
                
                return new ServiceResponse<List<TrialBalanceAccountsDto>>
                {
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = "No snapshot found for the given date range."
                };
            }

            var closingBalances = await _context.FinancialAccountSnapShots
                                        .Where(a => a.SnapShotDate.Date == lastSnap!.SnapShotDate.Date)  // Filter by the date
                                        .GroupBy(a => a.FinancialAccountId)  // Group by account ID
                                        .Select(g => g.OrderByDescending(a => a.SnapShotDate)  // Order by snapshot date in descending order
                                                      .FirstOrDefault())  // Select the latest snapshot per account
                                        .ToListAsync();

            var accounts = await _context.Accounts
                                    .Include(a => a.AccountCategory)
                                    .ToListAsync();

            var trialBalances = new List<TrialBalanceAccountsDto>();
            accounts.ForEach(accounts =>
            {
                var openingAccount = openingBalances.FirstOrDefault(a => a.FinancialAccountId == accounts.Id);
                var closingAccount = closingBalances.FirstOrDefault(a => a.FinancialAccountId == accounts.Id);

                var openingBalance = openingAccount != null ? openingAccount.ClosingBalance : 0;
                var closingBalance = closingAccount != null ? closingAccount.ClosingBalance : 0;

                var trialBalance = new TrialBalanceAccountsDto
                {
                    Name = accounts.Name,
                    Description = accounts.Description,
                    CreditBalance = accounts.AccountType == AccountType.Liability ? closingBalance - openingBalance
                                    : accounts.AccountType == AccountType.Equity ? closingBalance - openingBalance
                                    : accounts.AccountType == AccountType.Income ? closingBalance - openingBalance
                                    : null,
                    DebitBalance = accounts.AccountType == AccountType.Assets ? closingBalance - openingBalance
                                    : accounts.AccountType == AccountType.Expense ? closingBalance - openingBalance
                                    : null,
                    AccountType = accounts.AccountType,
                    AccountCategoryId = accounts.AccountCategoryId,
                    AccountCategoryName = accounts.AccountCategory!.Name
                };

                trialBalances.Add(trialBalance);
            });

            return new ServiceResponse<List<TrialBalanceAccountsDto>>
            {
                Time = DateTime.Now,
                IsSuccess = true,
                Data = trialBalances,
            };
        }
        catch (Exception ex) 
        {
            _logger.LogError("Failed to pull Trial Balance");
            return new ServiceResponse<List<TrialBalanceAccountsDto>>
            {
                Time = DateTime.Now,
                IsSuccess = false,
                Message = ex.Message,
            };
        }
    }

    public Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByMonthAsync(int month)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByYearAsync(int year)
    {
        throw new NotImplementedException();
    }
}