using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface ICashUpService
    {
        public ServiceResponse<CashUpDto> Create(List<CashUpDto> cashUp);
        public ServiceResponse<CashUpDto> Update(CashUpDto cashUp);
        public ServiceResponse<CashUpDto> Delete(int id);
        public ServiceResponse<IEnumerable<GetCashUpList>> GetByUserId(int id, DateTime date);
        public ServiceResponse<IEnumerable<GetCashUpList>> GetAll(DateTime date);
        public ServiceResponse<IEnumerable<GetReconListDto>> GetReconByUserId(int id, DateTime date);
        public ServiceResponse<IEnumerable<GetReconListDto>> GetAllRecon(DateTime date);
        Task<ServiceResponse<ICollection<CashUpReconDto>>> GetAllCashUpsByDate(DateTime date);
        Task<ServiceResponse<CashUpReconDto>> GetUserCashUpByDate(DateTime date, int userId);
    }
}
