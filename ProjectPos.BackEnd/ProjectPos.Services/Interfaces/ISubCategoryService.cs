using ProjectPos.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.Interfaces
{
    public interface ISubCategoryService
    {
        public ServiceResponse<SubCategoryDto> Create(SubCategoryDto subCategory);
        public ServiceResponse<SubCategoryDto> Update(SubCategoryDto subCategory);
        public ServiceResponse<SubCategoryDto> Delete(int id);
        public ServiceResponse<SubCategoryDto> GetById(int id);
        public ServiceResponse<List<SubCategoryDto>> GetByName(string name);
        public ServiceResponse<List<SubCategoryDto>> GetAll();
    }
}
