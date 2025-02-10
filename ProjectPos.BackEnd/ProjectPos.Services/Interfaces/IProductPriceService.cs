using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IProductPriceService
    {
        public ServiceResponse<ProductPriceDto> Create(ProductPriceDto price);
        public ServiceResponse<ProductPriceDto> Update(ProductPriceDto price);
        public ServiceResponse<ProductPriceDto> Delete(int id);
        public ServiceResponse<ProductPriceDto> GetById(int id);
        public ServiceResponse<List<ProductPriceDto>> GetByName(string name);
        public ServiceResponse<List<ProductPriceDto>> GetAll();
    }
}
