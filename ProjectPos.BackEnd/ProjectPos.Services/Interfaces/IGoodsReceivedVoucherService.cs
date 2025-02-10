using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IGoodsReceivedVoucherService
    {
        public ServiceResponse<GoodsReceivedVoucherDto> Create(GoodsReceivedVoucherDto grv);
        public Task<ServiceResponse<GoodsReceivedVoucherDto>> Update(GoodsReceivedVoucherDto grv);
        public Task<ServiceResponse<GoodsReceivedVoucherDto>> Approve(GoodsReceivedVoucherDto grv);
        public ServiceResponse<GoodsReceivedVoucherDto> Delete(int id);
        public ServiceResponse<GoodsReceivedVoucherDto> GetById(int id);
        public ServiceResponse<List<GoodsReceivedVoucherDto>> GetByName(string name);
        public ServiceResponse<List<GoodsReceivedVoucherDto>> GetAll();
        public Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllToday();
        public Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByDate(DateTime date);
        public Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByDateRange(DateTime start, DateTime end);
        public Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllBySupplier(int supplierId);
        public Task<ServiceResponse<List<GoodsReceivedVoucherDto>>> GetAllByMonth(int month);
    }
}
