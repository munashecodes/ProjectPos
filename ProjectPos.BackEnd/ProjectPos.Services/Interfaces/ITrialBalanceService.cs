using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface ITrialBalanceService
{
    Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByDateAsync(DateTime date);
    Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByMonthAsync(int month);
    Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByDateRangeAsync(DateTime start, DateTime end);
    Task<ServiceResponse<List<TrialBalanceAccountsDto>>> GetTrialBalanceByYearAsync(int year);
}