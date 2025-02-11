using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces
{
    public interface IUserService
    {
        public ServiceResponse<UserDto> Create(UserSignInDto user);
        public ServiceResponse<UserDto> Update(UserDto user);
        public ServiceResponse<UserDto> Delete(int id);
        public ServiceResponse<UserDto> Activate(UserDto user);
        public ServiceResponse<UserDto> GetById(int id);
        public ServiceResponse<bool> VerifySupervisor(string code);
        public ServiceResponse<List<UserDto>> GetByName(string name);
        public ServiceResponse<List<UserDto>> GetAll();
        public ServiceResponse<UserDto> Login(UserDto user);
        public ServiceResponse<UserDto> LogOut(UserDto user);
    }
}
