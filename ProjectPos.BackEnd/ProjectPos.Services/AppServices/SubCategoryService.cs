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
                    Message = "Customer Registered Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating subCategory");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
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
                    _logger.LogError($"SubCategory with id: {id} does not exist");
                    return new ServiceResponse<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Message = "SubCategory Not Found",
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
                        Message = $"subCategory {subCategory.Name} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting subCategory");
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
                    Message = $"Found {_subCategories.Count} SubCategories",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
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
                    _logger.LogError($"SubCategory with id: {id} does not exist");
                    return new ServiceResponse<SubCategoryDto>
                    {
                        IsSuccess = false,
                        Message = $"subCategory {id} Was Not Found",
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
                        Message = $"subCategory {subCategory.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting subCategory by id");
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
                    _logger.LogError($"SubCategory with name: {name} does not exist");
                    return new ServiceResponse<List<SubCategoryDto>>
                    {
                        IsSuccess = false,
                        Message = $"SubCategories with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _subCategories = _mapper.Map<List<SubCategory>, List<SubCategoryDto>>(companies);

                    return new ServiceResponse<List<SubCategoryDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_subCategories.Count} SubCategories",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting subCategory by name");
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
                    Message = "Customer UpDated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating subCategory");
                return new ServiceResponse<SubCategoryDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
