using ProjectPos.Services.DTOs;
using ProjectPos.Services.ReportingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IProofOfPaymentService
    {
        public ServiceResponse<ProofOfPaymentDto> Create(ProofOfPaymentDto pop);
        public ServiceResponse<ProofOfPaymentDto> Update(ProofOfPaymentDto pop);
        public ServiceResponse<ProofOfPaymentDto> Delete(int id);
        public ServiceResponse<ProofOfPaymentDto> Get(int id);
        public ServiceResponse<List<ProofOfPaymentDto>> GetAll();
        public ServiceResponse<List<ProofOfPaymentDto>> GetAllByCustomerId(int id);
        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAll(DateTime date);
        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAll(DateTime startDate, DateTime endDate);
        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetDateReportByCustomer(DateTime date);
        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetMonthReportByCustomer(int month);
        public ServiceResponse<IEnumerable<ProofOfPaymentReportDto>> GetRangeReportByCustomer(DateTime startDate, DateTime endDate);
        public ServiceResponse<IEnumerable<ProofOfPaymentDto>> GetAllMonth(int month);
    }
}
