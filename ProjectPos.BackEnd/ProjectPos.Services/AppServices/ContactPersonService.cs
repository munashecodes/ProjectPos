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
    public class ContactPersonService : IContactPersonService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactPersonService> _logger;

        public ContactPersonService(
            ProjectPosDbContext context,
            IMapper mapper,
            ILogger<ContactPersonService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public ServiceResponse<ContactPersonDto> Create(ContactPersonDto personDto)
        {
            try
            {
                var person = _mapper.Map<ContactPersonDto, ContactPerson>(personDto);
                var _person = _context.ContactPersons.Add(person);
                _context.SaveChanges();
                var persons = _context.ContactPersons!
                    .Include(x => x.Company)
                    .Include(x => x.Address)
                    .FirstOrDefault(c => c.Id == person.Id);
                
                return new ServiceResponse<ContactPersonDto>
                {
                    Data = _mapper.Map<ContactPerson, ContactPersonDto>(persons),
                    IsSuccess = true,
                    Message = $"Contact Person - {_person.Entity.FirstName} Created Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating contact person");
                return new ServiceResponse<ContactPersonDto>
                {
                    IsSuccess = false,
                    Message = $"Contact Person Addition Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ContactPersonDto> Delete(int id)
        {
            try
            {
                var person = _context.ContactPersons!.FirstOrDefault(x => x.Id == id);

                if (person == null)
                {
                    _logger.LogError($"Contact Person with id: {id} does not exist");
                    return new ServiceResponse<ContactPersonDto>
                    {
                        IsSuccess = false,
                        Message = "Contact Person Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    _context.ContactPersons!.Remove(person!);
                    _context.SaveChanges();
                    return new ServiceResponse<ContactPersonDto>
                    {
                        IsSuccess = true,
                        Message = $"person {person.FirstName} {person.LastName} Was deleted successfuly",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting person");
                return new ServiceResponse<ContactPersonDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ContactPersonDto>> GetAll()
        {
            try
            {
                var persons = _context.ContactPersons!
                    .Include(x => x.Company)
                    .Include(x => x.Address)
                    .ToList();
                var _persons = _mapper.Map<List<ContactPerson>, List<ContactPersonDto>>(persons);
                return new ServiceResponse<List<ContactPersonDto>>
                {
                    Data = _persons,
                    IsSuccess = true,
                    Message = $"Found {_persons.Count} Contact Persons",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all persons");
                return new ServiceResponse<List<ContactPersonDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ContactPersonDto> GetById(int id)
        {
            try
            {
                var person = _context.ContactPersons
                    .FirstOrDefault(c => c.Id == id);

                if (person == null)
                {
                    _logger.LogError($"Contact Person with id: {id} does not exist");
                    return new ServiceResponse<ContactPersonDto>
                    {
                        IsSuccess = false,
                        Message = $"person {id} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _person = _mapper.Map<ContactPerson, ContactPersonDto>(person);
                    return new ServiceResponse<ContactPersonDto>
                    {
                        Data = _person,
                        IsSuccess = true,
                        Message = $"person {person.FirstName} {person.LastName} Was Found",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting person by id");
                return new ServiceResponse<ContactPersonDto>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<List<ContactPersonDto>> GetByName(string name)
        {
            try
            {
                var persons = _context.ContactPersons
                    .Where(c => c.FirstName == name)
                    .ToList();

                if (persons == null)
                {
                    _logger.LogError($"ContactPerson with name: {name} does not exist");
                    return new ServiceResponse<List<ContactPersonDto>>
                    {
                        IsSuccess = false,
                        Message = $"ContactPersons with name {name} Was Not Found",
                        Time = DateTime.Now,
                    };
                }
                else
                {
                    var _persons = _mapper.Map<List<ContactPerson>, List<ContactPersonDto>>(persons);

                    return new ServiceResponse<List<ContactPersonDto>>
                    {
                        IsSuccess = true,
                        Message = $"Found {_persons.Count} ContactPersons",
                        Time = DateTime.Now,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting person by name");
                return new ServiceResponse<List<ContactPersonDto>>
                {
                    IsSuccess = false,
                    Message = $"Network Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }

        public ServiceResponse<ContactPersonDto> Update(ContactPersonDto personDto)
        {
            try
            {
                var person = _mapper.Map<ContactPersonDto, ContactPerson>(personDto);
                var _person = _context.ContactPersons!.Update(person);
                _context.SaveChanges();
                return new ServiceResponse<ContactPersonDto>
                {
                    Data = _mapper.Map<ContactPerson, ContactPersonDto>(_person.Entity),
                    IsSuccess = true,
                    Message = "Contact Person Updated Successfully",
                    Time = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating person");
                return new ServiceResponse<ContactPersonDto>
                {
                    IsSuccess = false,
                    Message = $"Contact Person Update Failed: {ex.Message}",
                    Time = DateTime.Now,
                };
            }
        }
    }
}
