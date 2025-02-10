using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IEmployeeService
    {
        public ServiceResponse<EmployeeDto> Create(EmployeeDto employee);
        public ServiceResponse<EmployeeDto> Delete(int id);
        public ServiceResponse<EmployeeDto> Update(EmployeeDto employee);
        public ServiceResponse<EmployeeDto> GetById(int id);
        public ServiceResponse<List<EmployeeDto>> GetByName(string name);
        public ServiceResponse<List<EmployeeDto>> GetAll();
    }
}
