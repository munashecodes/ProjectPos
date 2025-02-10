using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public string GenerateJwtToken(UserDto account);
        public bool ValidateJwtToken(UserDto token);
        //public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
