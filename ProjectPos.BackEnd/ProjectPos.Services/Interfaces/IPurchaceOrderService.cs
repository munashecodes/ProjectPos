using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IPurchaceOrderService
    {
        public ServiceResponse<PurchaceOrderDto> Create(PurchaceOrderDto order);
        public ServiceResponse<PurchaceOrderDto> Update(PurchaceOrderDto order);
        public ServiceResponse<PurchaceOrderDto> Delete(int id);
        public ServiceResponse<PurchaceOrderDto> GetById(int id);
        public ServiceResponse<List<PurchaceOrderDto>> GetByName(string name);
        public ServiceResponse<List<PurchaceOrderDto>> GetAll();
        public Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllToday();
        public Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByDate(DateTime date);
        public Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByDateRange(DateTime start, DateTime end);
        public Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllBySupplier(int supplierId);
        public Task<ServiceResponse<List<PurchaceOrderDto>>> GetAllByMonth(int month);
    }
}
