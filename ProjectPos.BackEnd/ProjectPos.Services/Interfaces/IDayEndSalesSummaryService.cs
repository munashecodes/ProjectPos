using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IDayEndSalesSummaryService
    {
        Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByDateAsync(DateTime date);
        Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByDateRangeAsync(DateTime start, DateTime end);
        Task<ServiceResponse<DayEndSalesSummaryDto>> GetDayByMonthAsync(int month);
        Task<ServiceResponse<DayEndSalesSummaryDto>> CreateDayEndSalesSummaryAsync();
        Task<ServiceResponse<ProductInventorySnapshotDto>> CreateInventorySnapShotAsync();
        Task<ServiceResponse<DayEndSalesSummaryDto>> UpdateDayEndSalesSummaryAsync(DayEndSalesSummaryDto dayEndSalesSummaryDto);
    }
}
