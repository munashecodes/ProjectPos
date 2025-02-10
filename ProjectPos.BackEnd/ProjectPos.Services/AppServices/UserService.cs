

using ProjectPos.Data;
using ProjectPos.Data.EntityModels;
using ProjectPos.Services.Interfaces;
using ProjectPos.Services.DTOs;
using AutoMapper;
using ProjectPos.Data.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using ProjectPos.Data.DbContexts;

namespace ProjectPos.Services.AppServices
{
    public class UserService : IUserService
    {
        private readonly ProjectPosDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UserService(
            ProjectPosDbContext dbContext,
            IAuthenticationService authenticationService, IMapper mapper)
        {
            _context = dbContext;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public ServiceResponse<UserDto> Activate(UserDto userDto)
        {
            try
            {
                var _user = _context.SystemUsers.FirstOrDefault(x => x.Id == userDto.Id);
                if (_user == null)
                    return new ServiceResponse<UserDto>
                    {
                        Time = DateTime.UtcNow,
                        Message = "user not registered",
                        IsSuccess = false
                    };
                else
                {
                    if (_user.IsActive == true)
                    {
                        return new ServiceResponse<UserDto>
                        {
                            Time = DateTime.UtcNow,
                            Message = "user already activated",
                            IsSuccess = false
                        };
                    }
                    else
                    {
                        _user.IsActive = userDto.IsActive;
                        _user.Role = userDto.Role;
                        _context.SaveChanges();
                        return new ServiceResponse<UserDto>
                        {
                            Time = DateTime.UtcNow,
                            Message = "user activated successfully",
                            IsSuccess = true
                        };
                    }
                }

            }
            catch
            {
                return new ServiceResponse<UserDto>
                {
                    Time = DateTime.UtcNow,
                    Message = "error activating the user",
                    IsSuccess = false
                };
            }
        }

        public ServiceResponse<bool> Create(UserSignInDto user)
        {
            if (_context.SystemUsers.Any(u => u.UserName == user.UserName))
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.UtcNow,
                    Message = "User Name or Email already exists",
                    IsSuccess = false,
                };
            }
            else
            {
                var _user = _mapper.Map<UserSignInDto, User>(user);
                var isFirstAccount = _context.SystemUsers.Count() == 0;

                _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                if(user.SupervisorCode != null)
                {
                    _user.SupervisorCodeHash = BCrypt.Net.BCrypt.HashPassword(user.SupervisorCode);
                }
                _user.Role = isFirstAccount ? Role.Manager : _user.Role;
                _user.IsActive = isFirstAccount ? true : _user.IsActive;

                _context.SystemUsers.Add(_user);
                _context.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    Time = DateTime.UtcNow,
                    Message = "Account created successfully",
                    IsSuccess = true
                };
            }
        }

        public ServiceResponse<UserDto> Delete(int id)
        {
            try
            {
                var emp = _context.SystemUsers!.FirstOrDefault(x => x.Id == id);

                if (emp == null)
                {
                    return new ServiceResponse<UserDto>
                    {
                        Message = $"User Number {id} Does not exist",
                        Time = DateTime.Now,
                        IsSuccess = false
                    };
                }
                else
                {
                    var res = _context.SystemUsers!.Remove(emp);
                    _context.SaveChanges();

                    return new ServiceResponse<UserDto>
                    {
                        Data = _mapper.Map<User, UserDto>(res.Entity),
                        Message = $"{res.Entity.FullName}'s Account Was Deleted Successfuly",
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

        public ServiceResponse<List<UserDto>> GetAll()
        {
            try
            {
                var users = _context.SystemUsers.ToList();
                var employees = _context.Employees.ToList();

                

                return new ServiceResponse<List<UserDto>>
                {
                    Data = _mapper.Map<List<User>, List<UserDto>>(users),
                    Message = "Retrieved  Users",
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<UserDto>>
                {
                    Message = ex.Message,
                    Time = DateTime.Now,
                    IsSuccess = true
                };
            }
        }

        public ServiceResponse<UserDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<List<UserDto>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<UserDto> Login(UserDto user)
        {
            try
            {
                var account = _context.SystemUsers!
                .FirstOrDefault(x => x.UserName == user.UserName);

                if (account == null)
                {
                    return new ServiceResponse<UserDto>
                    {
                        Time = DateTime.UtcNow,
                        Message = $"User {user.UserName} Does Not Exist",
                        IsSuccess = false,
                    };
                }
                else
                {
                    var _accessLog = _context.AccessLogs!.FirstOrDefault(
                        x => x.UserId == account.Id 
                        && x.IsLoggedIn == true 
                        && x.System == user.System);

                    if (_accessLog == null)
                    {
                        // validate
                        if (account == null || !account.IsActive || !BCrypt.Net.BCrypt.Verify(user.Password, account.PasswordHash))
                        {
                            return new ServiceResponse<UserDto>
                            {
                                Time = DateTime.UtcNow,
                                Message = "Email or Password Incorrect",
                                IsSuccess = false,
                            };
                        }
                        else if(account.Role == Role.Cashier && user.System == SystemName.BackOffice)
                        {
                            return new ServiceResponse<UserDto>
                            {
                                Time = DateTime.UtcNow,
                                Message = "Cashier Not Authorized To Access Back Office",
                                IsSuccess = false,
                            };
                        }
                        else
                        {
                            user.Id = account.Id;
                            user.Role = account.Role;
                            user.IsActive = account.IsActive;
                            account.JwtToken = _authenticationService.GenerateJwtToken(user);

                            var accessLog = new AccessLog
                            {
                                UserId = account.Id,
                                LogInTime = DateTime.Now,
                                LogOutTime = DateTime.Now,
                                IsLoggedIn = true,
                                JwtToken = account.JwtToken,
                                System = user.System
                            };

                            _context.AccessLogs!.Add(accessLog);

                            // save changes to db
                            _context.Update(account);
                            _context.SaveChanges();

                            var response = _mapper.Map<User, UserDto>(account);
                            //response.RefreshToken = refreshToken.Token;
                            return new ServiceResponse<UserDto>
                            {
                                Data = response,
                                Time = DateTime.UtcNow,
                                Message = "Logged in successfully",
                                IsSuccess = true,
                            };
                        }

                    }
                    else
                    {
                        return new ServiceResponse<UserDto>
                        {
                            Data = _mapper.Map<User, UserDto>(account),
                            Time = DateTime.UtcNow,
                            Message = "User is Already logged in. Make sure your previous sessions are logged off",
                            IsSuccess = false,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserDto>
                {
                    Time = DateTime.UtcNow,
                    Message = "User is Already logged in. Make sure your previous sessions are logged off",
                    IsSuccess = false,
                };
            }
        }

        public ServiceResponse<UserDto> LogOut(UserDto user)
        {
            try
            {
                var _accessLog = _context.AccessLogs.FirstOrDefault(
                    x => x.UserId == user.Id 
                    && x.IsLoggedIn == true
                    && x.System == user.System);

                if (_accessLog == null)
                {
                    return new ServiceResponse<UserDto>
                    {
                        Time = DateTime.UtcNow,
                        Message = "User already logged off",
                        IsSuccess = false,
                    };
                }
                else
                {
                    _accessLog.IsLoggedIn = false;
                    _accessLog.LogOutTime = DateTime.Now;

                    _context.AccessLogs.Update(_accessLog);
                    _context.SaveChanges();

                    return new ServiceResponse<UserDto>
                    {
                        Time = DateTime.UtcNow,
                        Message = "User Logged out Successfuly",
                        IsSuccess = true,
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ServiceResponse<UserDto> Update(UserDto userDto)
        {
            try
            {
                var user = _context.SystemUsers!.FirstOrDefault(x => x.Id == userDto.Id);

                user.Role = userDto.Role;
                user.IsActive = userDto.IsActive;
                user.PasswordHash = userDto.Password == null ? user.PasswordHash : BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                if (user.SupervisorCodeHash != null)
                {
                    _context.SystemUsers!.Update(user);
                    _context.SaveChanges();

                    var res = _mapper.Map<User, UserDto>(user);

                    return new ServiceResponse<UserDto>
                    {
                        Data = res,
                        Time = DateTime.UtcNow,
                        Message = "Account Updated successfully",
                        IsSuccess = true
                    };
                }
                else
                {
                    var now = DateTime.Now;

                    if (userDto.Role == Role.Manager || userDto.Role == Role.Supervisor)
                    {
                        do
                        {
                            userDto.SupervisorCode = (now - new DateTime(2000, 1, 1)).TotalDays.ToString();
                            userDto.SupervisorCode = userDto.SupervisorCode.Split('.')[1];
                        }
                        while (_context.SystemUsers!.Any(x => x.SupervisorCodeHash == userDto.SupervisorCode));

                        user.SupervisorCodeHash = userDto.SupervisorCode;

                    }

                    _context.SystemUsers!.Update(user);
                    _context.SaveChanges();

                    var res = _mapper.Map<User, UserDto>(user);

                    return new ServiceResponse<UserDto>
                    {
                        Data = res,
                        Time = DateTime.UtcNow,
                        Message = "Account Updated successfully",
                        IsSuccess = true
                    };
                }
                
                
            }
            catch (Exception ex)
            {
                return new ServiceResponse<UserDto>
                {
                    Time = DateTime.UtcNow,
                    Message = ex.Message,
                    IsSuccess = false
                };
            }
        }

        public ServiceResponse<bool> VerifySupervisor(string code)
        {
            try
            {
                var codeHash = BCrypt.Net.BCrypt.HashPassword(code);
                var canVerify = _context.SystemUsers!.Any(x => x.SupervisorCodeHash == code);
                if (canVerify)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = true,
                        Time = DateTime.UtcNow,
                        Message = "Supervisor Code Verified",
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Time = DateTime.UtcNow,
                        Message = "Supervisor Code Not Verified",
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
