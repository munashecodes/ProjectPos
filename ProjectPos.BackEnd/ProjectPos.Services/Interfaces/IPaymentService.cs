using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IPaymentService
    {
        public ServiceResponse<PaymentDto> Create(PaymentDto payment);
        public ServiceResponse<PaymentDto> Update(PaymentDto payment);
        public ServiceResponse<PaymentDto> Delete(int id);
        public ServiceResponse<PaymentDto> GetById(int id);
        public ServiceResponse<IEnumerable<PaymentDto>> GetByOrder(int orderId);
        public ServiceResponse<IEnumerable<PaymentDto>> GetAll();
    }
}
