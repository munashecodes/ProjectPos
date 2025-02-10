using ProjectPos.Services.DTOs;
using ProjectPos.Services.ReportingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface ISalesOrderService
    {
        public ServiceResponse<SalesOrderDto> Create(SalesOrderDto order);
        public ServiceResponse<SalesOrderDto> Update(SalesOrderDto order);
        public ServiceResponse<SalesOrderDto> Delete(int id);
        public ServiceResponse<SalesOrderDto> GetById(int id);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetByName(string name);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetTodayByName();
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetByCustomerId(int id);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll();
        public Task<ServiceResponse<IEnumerable<SalesOrderDto>>> GetAllToday();
        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems();
        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetMonthSalesOrderItems(int month);
        public ServiceResponse<IEnumerable<GroupedSalesOrderItemDto>> GetSalesOrderItems(DateTime date, int id);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll(DateTime date);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAll(DateTime startDate, DateTime endDate);
        public ServiceResponse<IEnumerable<GetSalesOrderByUserDto>> GetAllDate(DateTime date);
        public ServiceResponse<IEnumerable<GetSalesOrderByUserDto>> GetAllRange(DateTime startDate, DateTime endDate);
        public ServiceResponse<IEnumerable<SalesOrderDto>> GetAllMonth(int month);
        public ServiceResponse<IEnumerable<SalesEndDayReportDto>> GetByUserId();
    }
}
