using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class AccountCategoryService : IAccountCategoryService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;

        //constructor
        public AccountCategoryService(ProjectPosDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AccountCategoryDto>> CreateAsync(AccountCategoryDto item)
        {
            //try catch block returns a ServiceResponse object
            try
            {
                //map AccountCategoryDto to AccountCategory
                var accountCategory = _mapper.Map<AccountCategory>(item);

                //add the accountCategory object to the context
                _context.AccountCategories.Add(accountCategory);

                //save changes to the database
                await _context.SaveChangesAsync();

                //return a ServiceResponse object with the accountCategory object mapped to AccountCategoryDto
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    Data = _mapper.Map<AccountCategoryDto>(accountCategory), 
                    IsSuccess = true, 
                    Message = "Account Category created successfully",
                    Time = DateTime.UtcNow
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<AccountCategoryDto>> DeleteAsync(int id, int userId)
        {
            try
            {
                //find the accountCategory object by id
                var accountCategory = await _context.AccountCategories.FindAsync(id);

                //set isdeleted to true
                accountCategory.IsDeleted = true;
                accountCategory.DeleterId = userId;
                accountCategory.DeletionTime = DateTime.UtcNow;

                //remove the accountCategory object from the context
                _context.AccountCategories.Update(accountCategory);

                //save changes to the database
                await _context.SaveChangesAsync();

                //return a ServiceResponse object with the accountCategory object mapped to AccountCategoryDto
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    Data = _mapper.Map<AccountCategoryDto>(accountCategory), 
                    IsSuccess = true, 
                    Message = "Account Category deleted successfully",
                    Time = DateTime.UtcNow
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<List<AccountCategoryDto>>> GetAllAsync()
        {
            try
            {
                //get all accountCategory objects from the context
                var accountCategories = await _context.AccountCategories
                    .Include(cat => cat.Creator)
                    .Include(cat => cat.Deleter)
                    .ToListAsync();

                //return a ServiceResponse object with the accountCategory objects mapped to AccountCategoryDto
                return new ServiceResponse<List<AccountCategoryDto>> 
                { 
                    Data = _mapper.Map<List<AccountCategoryDto>>(accountCategories), 
                    IsSuccess = true, 
                    Message = "Account Categories retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AccountCategoryDto>> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<AccountCategoryDto>> GetByIdAsync(int id)
        {
            try
            {
                //find the accountCategory object by id
                var accountCategory = await _context.AccountCategories
                    .Include(cat => cat.Creator)
                    .Include(cat => cat.Deleter)
                    .FirstOrDefaultAsync(cat => cat.Id == id);

                //return a ServiceResponse object with the accountCategory object mapped to AccountCategoryDto
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    Data = _mapper.Map<AccountCategoryDto>(accountCategory), 
                    IsSuccess = true, 
                    Message = "Account Category retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<List<AccountCategoryDto>>> GetByNameAsync(string name)
        {
            try
            {
                //get all accountCategory objects from the context where the name matches the name parameter
                var accountCategories = await _context.AccountCategories
                    .Include(cat => cat.Creator)
                    .Include(cat => cat.Deleter)
                    .Where(cat => cat.Name == name)
                    .ToListAsync();

                //return a ServiceResponse object with the accountCategory objects mapped to AccountCategoryDto
                return new ServiceResponse<List<AccountCategoryDto>> 
                { 
                    Data = _mapper.Map<List<AccountCategoryDto>>(accountCategories), 
                    IsSuccess = true, 
                    Message = "Account Categories retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AccountCategoryDto>> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<AccountCategoryDto>> UpdateAsync(AccountCategoryDto item)
        {
            try
            {
                //map AccountCategoryDto to AccountCategory
                var accountCategory = _mapper.Map<AccountCategory>(item);

                //update the accountCategory object in the context
                _context.AccountCategories.Update(accountCategory);

                //save changes to the database
                await _context.SaveChangesAsync();

                //return a ServiceResponse object with the accountCategory object mapped to AccountCategoryDto
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    Data = _mapper.Map<AccountCategoryDto>(accountCategory), 
                    IsSuccess = true, 
                    Message = "Account Category updated successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AccountCategoryDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }
    }
}
