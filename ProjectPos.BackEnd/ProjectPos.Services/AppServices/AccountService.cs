using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectPos.Data.DbContexts;
using ProjectPos.Data.EntityModels;
using ProjectPos.Data.Shared.Enums;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Services.AppServices
{
    public class AccountService : IAccountService
    {
        //inject dbcontext and IMapper here
        private readonly IMapper _mapper;
        private readonly ProjectPosDbContext _context;
        private ILogger<AccountService> _logger;

        public AccountService(
            ProjectPosDbContext context, 
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<AccountDto>> CreateAsync(AccountDto item)
        {
            try
            {
                var account = _mapper.Map<Account>(item);
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();

                return new ServiceResponse<AccountDto> 
                { 
                    IsSuccess = true, 
                    Data = _mapper.Map<AccountDto>(account) ,
                    Message = "Account created successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<AccountDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<AccountDto>> DeleteAsync(int id, int userId)
        {
            try
            {
                //find account by id
                var account = await _context.Accounts.FindAsync(id);
                if (account == null)
                {
                    return new ServiceResponse<AccountDto> 
                    { 
                        IsSuccess = false, 
                        Message = "Account not found" 
                    };
                }

                //update isdeleted flag to true
                account.IsDeleted = true;
                account.DeleterId = userId;
                account.DeletionTime = DateTime.UtcNow;

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return new ServiceResponse<AccountDto> 
                { 
                    IsSuccess = true, 
                    Data = _mapper.Map<AccountDto>(account) ,
                    Message = "Account deleted successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<AccountDto> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<List<AccountDto>>> GetAllAsync()
        {
            try
            {
                //get all accounts including categories, creater,deleter and last modifier. 
                var accounts = await _context.Accounts
                    .Include(x => x.AccountCategory)
                    .Include(x => x.Creator)
                    .Include(x => x.Deleter)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();

                return new ServiceResponse<List<AccountDto>>
                {
                    Data = _mapper.Map<List<AccountDto>>(accounts),
                    IsSuccess = true,
                    Message = "Accounts retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<AccountDto>> 
                { 
                    IsSuccess = false, 
                    Message = ex.Message 
                };
            }
        }

        public async Task<ServiceResponse<List<AccountDto>>> GetByCategoryAsync(int catId)
        {
            try
            {
                //get all accounts including categories, creater,deleter and last modifier. 
                var accounts = await _context.Accounts
                    .Include(x => x.AccountCategory)
                    .Include(x => x.Creator)
                    .Include(x => x.Deleter)
                    .Where(x => x.IsDeleted == false && x.AccountCategoryId == catId)
                    .ToListAsync();

                return new ServiceResponse<List<AccountDto>>
                {
                    Data = _mapper.Map<List<AccountDto>>(accounts),
                    IsSuccess = true,
                    Message = "Accounts retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<AccountDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<AccountDto>> GetByIdAsync(int id)
        {
            try
            {
                //get all accounts including categories, creater,deleter and last modifier. 
                var accounts = await _context.Accounts
                    .Include(x => x.AccountCategory)
                    .Include(x => x.Creator)
                    .Include(x => x.Deleter)
                    .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);

                return new ServiceResponse<AccountDto>
                {
                    Data = _mapper.Map<AccountDto>(accounts),
                    IsSuccess = true,
                    Message = "Accounts retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<AccountDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<List<AccountDto>>> GetByTypeAsync(AccountType type)
        {
            try
            {
                //get all accounts including categories, creater,deleter and last modifier. 
                var accounts = await _context.Accounts
                    .Include(x => x.AccountCategory)
                    .Include(x => x.Creator)
                    .Include(x => x.Deleter)
                    .Where(x => x.IsDeleted == false && x.AccountType == type)
                    .ToListAsync();

                return new ServiceResponse<List<AccountDto>>
                {
                    Data = _mapper.Map<List<AccountDto>>(accounts),
                    IsSuccess = true,
                    Message = "Accounts retrieved successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<AccountDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<AccountDto>> UpdateAsync(AccountDto item)
        {
            try
            {
                var account = _mapper.Map<Account>(item);
                 var res = _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return new ServiceResponse<AccountDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<AccountDto>(res),
                    Message = "Account created successfully",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                //log error here
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<AccountDto>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
