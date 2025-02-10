using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface IContactPersonService
    {
        public ServiceResponse<ContactPersonDto> Create(ContactPersonDto person);
        public ServiceResponse<ContactPersonDto> Update(ContactPersonDto person);
        public ServiceResponse<ContactPersonDto> Delete(int id);
        public ServiceResponse<ContactPersonDto> GetById(int id);
        public ServiceResponse<List<ContactPersonDto>> GetByName(string name);
        public ServiceResponse<List<ContactPersonDto>> GetAll();
    }
}
