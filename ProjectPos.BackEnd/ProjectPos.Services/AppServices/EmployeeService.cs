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
    public class EmployeeService : IEmployeeService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IMapper _mapper;
        public EmployeeService(ProjectPosDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ServiceResponse<EmployeeDto> Create(EmployeeDto employee)
        {
            try
            {
                var emp = _context.Employees!.FirstOrDefault(x => x.NatId == employee.NatId);

                if (emp != null)
                {
                    return new ServiceResponse<EmployeeDto>
                    {
                        Message = $"Employee with ID Number {employee.NatId} already exist",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    var _employee = _mapper.Map<EmployeeDto, Employee>(employee);
                    var res = _context.Employees.Add(_employee);
                    _context.SaveChanges();

                    return new ServiceResponse<EmployeeDto>
                    {
                        Data = _mapper.Map<Employee, EmployeeDto>(res.Entity),
                        Message = $"{res.Entity.Name}'s Profile Was Created Successfuly",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<EmployeeDto> Delete(int id)
        {
            try
            {
                var emp = _context.Employees!.FirstOrDefault(x => x.Id == id);

                if (emp == null)
                {
                    return new ServiceResponse<EmployeeDto>
                    {
                        Message = $"Employee Number {id} Does not exist",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    var res = _context.Employees!.Remove(emp);
                    _context.SaveChanges();

                    return new ServiceResponse<EmployeeDto>
                    {
                        Data = _mapper.Map<Employee, EmployeeDto>(res.Entity),
                        Message = $"{res.Entity.Name}'s Profile Was Deleted Successfuly",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<List<EmployeeDto>> GetAll()
        {
            try
            {
                var employees = _context.Employees
                    .Include(a => a.Address)
                    .ToList();
                var res = _mapper.Map<List<Employee>, List<EmployeeDto>>(employees);

                return new ServiceResponse<List<EmployeeDto>>
                {
                    Data = res,
                    Message = $"Retrieved {res.Count} Employees",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<EmployeeDto> GetById(int id)
        {
            try
            {
                var employee = _context.Employees!
                    .FirstOrDefault(x => x.Id == id);

                if (employee == null)
                {
                    return new ServiceResponse<EmployeeDto>
                    {
                        Data = null,
                        Message = $"Employee Number {id} Not Available",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    var res = _mapper.Map<Employee, EmployeeDto>(employee);
                    return new ServiceResponse<EmployeeDto>
                    {
                        Data = res,
                        Message = $"Employee Number {id} Found",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<List<EmployeeDto>> GetByName(string name)
        {
            try
            {
                var employees = _context.Employees!
                    .Where(x => x.Name == name)
                    .ToList();

                if (employees == null)
                {
                    return new ServiceResponse<List<EmployeeDto>>
                    {
                        Data = null,
                        Message = $"No Employees With Name {name} Where Available",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    var res = _mapper.Map<List<Employee>, List<EmployeeDto>>(employees);
                    return new ServiceResponse<List<EmployeeDto>>
                    {
                        Data = res,
                        Message = $"{res.Count} Employees With Name {name} Found",
                        Time = DateTime.Now,
                        IsSuccess = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<EmployeeDto> Update(EmployeeDto employee)
        {
            try
            {
                var _employee = _mapper.Map<EmployeeDto, Employee>(employee);
                var res = _context.Employees.Update(_employee);
                _context.SaveChanges();

                return new ServiceResponse<EmployeeDto>
                {
                    Data = _mapper.Map<Employee, EmployeeDto>(res.Entity),
                    Message = $"{res.Entity.Name}'s Profile Was Updated Successfuly",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
