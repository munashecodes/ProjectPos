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
    public class ProductPriceService : IProductPriceService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductPriceService> _logger;

        public ProductPriceService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<ProductPriceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<ProductPriceDto> Create(ProductPriceDto priceDto)
        {
            try
            {
                var price = _mapper.Map<ProductPriceDto, ProductPrice>(priceDto);
                price.CreationTime = DateTime.Now;
                var _price = _context.ProductPrices!.Add(price);
                _context.SaveChanges();
                var newPrice = _context.ProductPrices
                    .Include(x => x.Product)
                    .FirstOrDefault(p => p.Id == _price.Entity.Id);
                return new ServiceResponse<ProductPriceDto>
                {
                    Data = _mapper.Map<ProductPrice, ProductPriceDto>(newPrice),
                    IsSuccess = true,
                    Message = "Price Created Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating price");
                return new ServiceResponse<ProductPriceDto>
                {
                    IsSuccess = false,
                    Message = $"Price Creation Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductPriceDto> Delete(int id)
        {
            try
            {
                var price = _context.ProductPrices!
                    .Include(x => x.Product)
                    .FirstOrDefault(x => x.Id == id);

                if (price == null)
                {
                    _logger.LogError($"Product Price with id: {id} does not exist");
                    return new ServiceResponse<ProductPriceDto>
                    {
                        IsSuccess = false,
                        Message = "Product Price Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.ProductPrices!.Remove(price!);
                    _context.SaveChanges();
                    return new ServiceResponse<ProductPriceDto>
                    {
                        IsSuccess = true,
                        Message = $"Price {price.Product!.Name} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Price");
                return new ServiceResponse<ProductPriceDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ProductPriceDto>> GetAll()
        {
            try
            {
                var companies = _context.ProductPrices
                    .Include(x => x.Product)
                    .ToList();
                var _companies = _mapper.Map<List<ProductPrice>, List<ProductPriceDto>>(companies);
                return new ServiceResponse<List<ProductPriceDto>>
                {
                    Data = _companies,
                    IsSuccess = true,
                    Message = $"Found {_companies.Count} Product Prices",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all prices");
                return new ServiceResponse<List<ProductPriceDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductPriceDto> GetById(int id)
        {
            try
            {
                //ProductPrice = _mapper.Map<ProductPrice, ProductPriceDto>(
                //        _context.ProductPrices!
                //        .OrderBy(a => a.CreationTime.Date)
                //        .LastOrDefault(a => a.ProductInventoryId == x.Id)!
                //        ),

                var price = _context.ProductPrices!
                        .Include(x => x.Product)
                        .OrderBy(a => a.CreationTime)
                        .LastOrDefault(a => a.ProductInventoryId == id);

                if (price == null)
                {
                    _logger.LogError($"Product Price with id: {id} does not exist");
                    return new ServiceResponse<ProductPriceDto>
                    {
                        IsSuccess = false,
                        Message = $"Price {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _price = _mapper.Map<ProductPrice, ProductPriceDto>(price);
                    return new ServiceResponse<ProductPriceDto>
                    {
                        Data = _price,
                        IsSuccess = true,
                        Message = $"Price for {_price.Name} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting price by id");
                return new ServiceResponse<ProductPriceDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ProductPriceDto>> GetByName(string name)
        {
            try
            {
                var companies = _context.ProductPrices!
                    .Where(c => c.Product!.Name == name)
                    .ToList();

                if (companies == null)
                {
                    _logger.LogError($"Product Price with name: {name} does not exist");
                    return new ServiceResponse<List<ProductPriceDto>>
                    {
                        IsSuccess = false,
                        Message = $"Product Prices with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _companies = _mapper.Map<List<ProductPrice>, List<ProductPriceDto>>(companies);

                    return new ServiceResponse<List<ProductPriceDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_companies.Count} ProductPrices",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting price by name");
                return new ServiceResponse<List<ProductPriceDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ProductPriceDto> Update(ProductPriceDto priceDto)
        {
            try
            {
                var price = _mapper.Map<ProductPriceDto, ProductPrice>(priceDto);
                var updatedPrice = _context.ProductPrices!.Update(price);
                _context.SaveChanges();
                var _price = _context.ProductPrices!
                    .Include(x => x.Product)
                    .FirstOrDefault(p => p.Id == updatedPrice.Entity.Id);
                return new ServiceResponse<ProductPriceDto>
                {
                    Data = _mapper.Map<ProductPrice, ProductPriceDto>(_price),
                    IsSuccess = true,
                    Message = "Price Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating price");
                return new ServiceResponse<ProductPriceDto>
                {
                    IsSuccess = false,
                    Message = $"Customer Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
