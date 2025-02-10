using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface ICompanyService
    {
        public ServiceResponse<CompanyDto> Create(CompanyDto company);
        public ServiceResponse<CompanyDto> Update(CompanyDto company);
        public ServiceResponse<CompanyDto> Delete(int id);
        public ServiceResponse<CompanyDto> GetById(int id);
        public ServiceResponse<List<CompanyDto>> GetByName(string name);
        public ServiceResponse<List<CompanyDto>> GetAll();
    }
}
