using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IStockMovementService
    {
        Task<ServiceResponse<List<StockMovementDto>>> AddRangeAsync(List<StockMovementDto> stockMovement);
        Task<ServiceResponse<StockMovementLogDto>> AddAsync(StockMovementLogDto stockMovement);
        Task<ServiceResponse<StockMovementLogDto>> Approve(int id, int userId);
        Task<ServiceResponse<StockMovementLogDto>> UpdateAsync(StockMovementLogDto stockMovement);
        Task<ServiceResponse<StockMovementLogDto>> DeleteAsync(int id, int userId);
        Task<ServiceResponse<StockMovementLogDto>> GetByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllAsync();
        Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllTodayAsync();
        Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByDateAsync(DateTime date );
        Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ServiceResponse<IEnumerable<StockMovementLogDto>>> GetAllByMonthAsync(int month);
    }
}
