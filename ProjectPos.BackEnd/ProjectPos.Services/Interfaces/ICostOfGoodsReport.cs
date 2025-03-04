using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface ICostOfGoodsReport
{
    Task<ServiceResponse<CogsReportDto>> GetCogsReport(DateOnly? startDate, DateOnly? endDate);
    Task<ServiceResponse<CostOfGoodsReportDto>> GetCostOfGoodsReport();
}