using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IPurchaceInvoiceService
    {
        public ServiceResponse<PurchaceInvoiceDto> Create(PurchaceInvoiceDto invoice);
        public ServiceResponse<PurchaceInvoiceDto> Update(PurchaceInvoiceDto invoice);
        public ServiceResponse<PurchaceInvoiceDto> Delete(int id);
        public ServiceResponse<PurchaceInvoiceDto> GetById(int id);
        public ServiceResponse<List<PurchaceInvoiceDto>> GetByName(string name);
        public ServiceResponse<List<PurchaceInvoiceDto>> GetAll();
    }
}
