using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ProjectPos.Services.Interfaces;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.DTOs;
using ProjectPos.Data.EntityModels;
using Microsoft.Extensions.Logging;

namespace ProjectPos.Services.AppServices
{
    public class CompanyService : ICompanyService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;
        
        public CompanyService(
            ProjectPosDbContext context, 
            IMapper mapper,
            ILogger<CompanyService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<CompanyDto> Create(CompanyDto companyDto)
        {
            try
            {
                var company = _mapper.Map<CompanyDto, Company>(companyDto);
                var _company = _context.Companies.Add(company);
                _context.SaveChanges();
                return new ServiceResponse<CompanyDto>
                {
                    Data = _mapper.Map<Company, CompanyDto>(_company.Entity),
                    IsSuccess = true,
                    Message = "Customer Registered Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating company");
                return new ServiceResponse<CompanyDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<CompanyDto> Delete(int id)
        {
            try
            {
                var company = _context.Companies!.FirstOrDefault(x => x.Id == id);

                if (company == null)
                {
                    _logger.LogError($"Company with id: {id} does not exist");
                    return new ServiceResponse<CompanyDto>
                    {
                        IsSuccess = false,
                        Message = "Company Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.Companies!.Remove(company!);
                    _context.SaveChanges();
                    return new ServiceResponse<CompanyDto>
                    {
                        IsSuccess = true,
                        Message = $"company {company.Name} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting company");
                return new ServiceResponse<CompanyDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<CompanyDto>> GetAll()
        {
            try
            {
                var companies = _context.Companies
                    .Include(x => x.Address)
                    .ToList();
                var _companies = _mapper.Map<List<Company>, List<CompanyDto>>(companies);
                return new ServiceResponse<List<CompanyDto>>
                {
                    Data = _companies,
                    IsSuccess = true,
                    Message = $"Found {_companies.Count} Companies",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all companies");
                return new ServiceResponse<List<CompanyDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<CompanyDto> GetById(int id)
        {
            try
            {
                var company = _context.Companies
                    .FirstOrDefault(c => c.Id == id);

                if (company == null)
                {
                    _logger.LogError($"Company with id: {id} does not exist");
                    return new ServiceResponse<CompanyDto>
                    {
                        IsSuccess = false,
                        Message = $"company {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _company = _mapper.Map<Company, CompanyDto>(company);
                    return new ServiceResponse<CompanyDto>
                    {
                        Data = _company,
                        IsSuccess = true,
                        Message = $"company {company.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting company by id");
                return new ServiceResponse<CompanyDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<CompanyDto>> GetByName(string name)
        {
            try
            {
                var companies = _context.Companies
                    .Where(c => c.Name == name)
                    .ToList();

                if (companies == null)
                {
                    _logger.LogError($"Company with name: {name} does not exist");
                    return new ServiceResponse<List<CompanyDto>>
                    {
                        IsSuccess = false,
                        Message = $"Companies with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _companies = _mapper.Map<List<Company>, List<CompanyDto>>(companies);

                    return new ServiceResponse<List<CompanyDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_companies.Count} Companies",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting company by name");
                return new ServiceResponse<List<CompanyDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<CompanyDto> Update(CompanyDto companyDto)
        {
            try
            {
                var company = _mapper.Map<CompanyDto, Company>(companyDto);
                var _company = _context.Companies!.Update(company);
                _context.SaveChanges();
                return new ServiceResponse<CompanyDto>
                {
                    Data = _mapper.Map<Company, CompanyDto>(_company.Entity),
                    IsSuccess = true,
                    Message = "Customer UpDated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating company");
                return new ServiceResponse<CompanyDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
