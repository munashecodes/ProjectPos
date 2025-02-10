using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IProductInventoryService
    {
        public ServiceResponse<ProductInventoryDto> Create(ProductInventoryDto inventory);
        public Task UpdateProductInventoryStatus();
        public ServiceResponse<ProductInventoryDto> Update(ProductInventoryDto inventory);

        public ServiceResponse<List<ProductInventoryDto>> UpdateRange(List<ProductInventoryDto> inventory);
        public ServiceResponse<ProductInventoryDto> Delete(int id);
        public ServiceResponse<ProductInventoryDto> GetById(int id);
        public ServiceResponse<ProductInventoryDto> GetByPlu(string plu);
        public ServiceResponse<ProductInventoryDto> GetByName(string name);
        public ServiceResponse<IEnumerable<ProductInventoryDto>> GetAll();
        public ServiceResponse<IEnumerable<ProductInventoryDto>> GetAllToSell();
        public Task<ServiceResponse<IEnumerable<ProductInventoryDto>>> GenerateInventory();
        public Task<ServiceResponse<IEnumerable<StockTakeReportDto>>> GenerateStockTakeReport(Category department);
        public Task<ServiceResponse<IEnumerable<StockValueReportDto>>> GenerateStockValueReport(Category department);
        public Task<ServiceResponse<StockTakeLogDto>> CreateStockTakeLog(StockTakeLogDto log);
        public Task<ServiceResponse<StockTakeLogDto>> CloseStockTakeLog(StockTakeLogDto log);
        public Task<ServiceResponse<StockTakeLogDto>> CheckStockTakeLog(Category department);
    }
}
