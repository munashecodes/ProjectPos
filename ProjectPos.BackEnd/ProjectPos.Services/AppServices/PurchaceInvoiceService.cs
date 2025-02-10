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
    public class PurchaceInvoiceService : IPurchaceInvoiceService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaceInvoiceService> _logger;

        public PurchaceInvoiceService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<PurchaceInvoiceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<PurchaceInvoiceDto> Create(PurchaceInvoiceDto purchaceInvoiceDto)
        {
            try
            {
                var purchaceInvoice = _mapper.Map<PurchaceInvoiceDto, PurchaceInvoice>(purchaceInvoiceDto);
                var _purchaceInvoice = _context.PurchaceInvoices.Add(purchaceInvoice);
                _context.SaveChanges();
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    Data = _mapper.Map<PurchaceInvoice, PurchaceInvoiceDto>(_purchaceInvoice.Entity),
                    IsSuccess = true,
                    Message = "Customer Registered Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating purchaceInvoice");
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Registration Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceInvoiceDto> Delete(int id)
        {
            try
            {
                var purchaceInvoice = _context.PurchaceInvoices!.FirstOrDefault(x => x.Id == id);

                if (purchaceInvoice == null)
                {
                    _logger.LogError($"PurchaceInvoice with id: {id} does not exist");
                    return new ServiceResponse<PurchaceInvoiceDto>
                    {
                        IsSuccess = false,
                        Message = "PurchaceInvoice Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.PurchaceInvoices!.Remove(purchaceInvoice!);
                    _context.SaveChanges();
                    return new ServiceResponse<PurchaceInvoiceDto>
                    {
                        IsSuccess = true,
                        Message = $"purchaceInvoice {purchaceInvoice.Supplier} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting purchaceInvoice");
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<PurchaceInvoiceDto>> GetAll()
        {
            try
            {
                var purchaceInvoices = _context.PurchaceInvoices
                    .Include(x => x.InvoiceLines)
                    .ToList();
                var _purchaceInvoices = _mapper.Map<List<PurchaceInvoice>, List<PurchaceInvoiceDto>>(purchaceInvoices);
                return new ServiceResponse<List<PurchaceInvoiceDto>>
                {
                    IsSuccess = true,
                    Message = $"Found {_purchaceInvoices.Count} PurchaceInvoices",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all purchaceInvoices");
                return new ServiceResponse<List<PurchaceInvoiceDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceInvoiceDto> GetById(int id)
        {
            try
            {
                var purchaceInvoice = _context.PurchaceInvoices
                    .FirstOrDefault(c => c.Id == id);

                if (purchaceInvoice == null)
                {
                    _logger.LogError($"PurchaceInvoice with id: {id} does not exist");
                    return new ServiceResponse<PurchaceInvoiceDto>
                    {
                        IsSuccess = false,
                        Message = $"purchaceInvoice {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _purchaceInvoice = _mapper.Map<PurchaceInvoice, PurchaceInvoiceDto>(purchaceInvoice);
                    return new ServiceResponse<PurchaceInvoiceDto>
                    {
                        Data = _purchaceInvoice,
                        IsSuccess = true,
                        Message = $"purchaceInvoice {purchaceInvoice.Supplier} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting purchaceInvoice by id");
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<PurchaceInvoiceDto>> GetByName(string name)
        {
            try
            {
                var purchaceInvoices = _context.PurchaceInvoices
                    .Where(c => c.Supplier == name)
                    .ToList();

                if (purchaceInvoices == null)
                {
                    _logger.LogError($"PurchaceInvoice with name: {name} does not exist");
                    return new ServiceResponse<List<PurchaceInvoiceDto>>
                    {
                        IsSuccess = false,
                        Message = $"PurchaceInvoices with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _purchaceInvoices = _mapper.Map<List<PurchaceInvoice>, List<PurchaceInvoiceDto>>(purchaceInvoices);

                    return new ServiceResponse<List<PurchaceInvoiceDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_purchaceInvoices.Count} PurchaceInvoices",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting purchaceInvoice by name");
                return new ServiceResponse<List<PurchaceInvoiceDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<PurchaceInvoiceDto> Update(PurchaceInvoiceDto purchaceInvoiceDto)
        {
            try
            {
                var purchaceInvoice = _mapper.Map<PurchaceInvoiceDto, PurchaceInvoice>(purchaceInvoiceDto);
                var _purchaceInvoice = _context.PurchaceInvoices!.Update(purchaceInvoice);
                _context.SaveChanges();
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    Data = _mapper.Map<PurchaceInvoice, PurchaceInvoiceDto>(_purchaceInvoice.Entity),
                    IsSuccess = true,
                    Message = "Customer UpDated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating purchaceInvoice");
                return new ServiceResponse<PurchaceInvoiceDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
