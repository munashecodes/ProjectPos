using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryService> _logger;

        public SubCategoryService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<SubCategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<SubCategoryDto> Create(SubCategoryDto subCategoryDto)
        {
            try
            {
                var subCategory = _mapper.Map<SubCategoryDto, SubCategory>(subCategoryDto);
                var _subCategory = _context.SubCategories!.Add(subCategory);
                _context.SaveChanges();
                return new ServiceResponse<SubCategoryDto>
                {
                    Data = _mapper.Map<SubCategory, SubCategoryDto>(_subCategory.Entity),
                    IsSuccess = true,
                    Message = "Sub Category Added Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Sub Category");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Sub Category Addition Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SubCategoryDto> Delete(int id)
        {
            try
            {
                var subCategory = _context.SubCategories!.FirstOrDefault(x => x.Id == id);

                if (subCategory == null)
                {
                    _logger.LogError($"Sub Category with id: {id} does not exist");
                    return new ServiceResponse<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Message = "Sub Category Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.SubCategories!.Remove(subCategory!);
                    _context.SaveChanges();
                    return new ServiceResponse<SubCategoryDto>
                    {
                        IsSuccess = true,
                        Message = $"Sub Category {subCategory.Name} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Sub Category");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<SubCategoryDto>> GetAll()
        {
            try
            {
                var companies = _context.SubCategories!
                    .ToList();
                var _subCategories = _mapper.Map<List<SubCategory>, List<SubCategoryDto>>(companies);
                return new ServiceResponse<List<SubCategoryDto>>
                {
                    Data = _subCategories,
                    IsSuccess = true,
                    Message = $"Found {_subCategories.Count} Sub Categories",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all sub categories");
                return new ServiceResponse<List<SubCategoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SubCategoryDto> GetById(int id)
        {
            try
            {
                var subCategory = _context.SubCategories!
                    .FirstOrDefault(c => c.Id == id);

                if (subCategory == null)
                {
                    _logger.LogError($"Sub Category with id: {id} does not exist");
                    return new ServiceResponse<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Message = $"Sub Category {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _subCategory = _mapper.Map<SubCategory, SubCategoryDto>(subCategory);
                    return new ServiceResponse<SubCategoryDto>
                    {
                        Data = _subCategory,
                        IsSuccess = true,
                        Message = $"Sub Category {subCategory.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Sub Category by id");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<SubCategoryDto>> GetByName(string name)
        {
            try
            {
                var companies = _context.SubCategories!
                    .Where(c => c.Name == name)
                    .ToList();

                if (companies == null)
                {
                    _logger.LogError($"Sub Category with name: {name} does not exist");
                    return new ServiceResponse<List<SubCategoryDto>>
                    {
                        IsSuccess = false,
                        Message = $"Sub Categories with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _subCategories = _mapper.Map<List<SubCategory>, List<SubCategoryDto>>(companies);

                    return new ServiceResponse<List<SubCategoryDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_subCategories.Count} Sub Categories",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Sub Category by name");
                return new ServiceResponse<List<SubCategoryDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<SubCategoryDto> Update(SubCategoryDto subCategoryDto)
        {
            try
            {
                var subCategory = _mapper.Map<SubCategoryDto, SubCategory>(subCategoryDto);
                var _subCategory = _context.SubCategories!.Update(subCategory);
                _context.SaveChanges();
                return new ServiceResponse<SubCategoryDto>
                {
                    Data = _mapper.Map<SubCategory, SubCategoryDto>(_subCategory.Entity),
                    IsSuccess = true,
                    Message = "Sub Category Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Sub Category");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Sub Category Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
