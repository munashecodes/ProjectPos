using ProjectPos.Services.DTOs;

namespace ProjectPos.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserDto user);
        public int? ValidateJwtToken(string token);
        //public RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
