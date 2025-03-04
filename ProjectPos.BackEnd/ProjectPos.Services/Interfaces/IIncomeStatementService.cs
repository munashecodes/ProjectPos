using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IIncomeStatementService
{
    Task<ServiceResponse<IncomeStatementDto>> GenerateIncomeStatementAsync(DateTime startDate, DateTime endDate);
}