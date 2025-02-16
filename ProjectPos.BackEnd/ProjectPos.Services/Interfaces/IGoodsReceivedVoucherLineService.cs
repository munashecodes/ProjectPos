using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces;

public interface IGoodsReceivedVoucherLineService
{
    public ServiceResponse<List<GroupedGrvItemsDto>> GetTodayGrvItems();
    public ServiceResponse<List<GroupedGrvItemsDto>> GetByMonthGrvItems(int month);
    public ServiceResponse<List<GroupedGrvItemsDto>> GetByDateGrvItems(DateTime date);
    public ServiceResponse<List<GroupedGrvItemsDto>> GetGrvItemsByRange(DateTime start, DateTime end);
}