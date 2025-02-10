using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectPos.Data;
using ProjectPos.Data.DbContexts;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPos.Services.AppServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ProjectPosDbContext _context;
        public IConfiguration _configuration;
        public AuthenticationService(
            ProjectPosDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserDto account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateJwtToken(UserDto user)
        {
            if (user == null || user.JwtToken == null || user.JwtToken == "")
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                var _accessLog = _context.AccessLogs.FirstOrDefault(x => x.UserId == user.Id && x.IsLoggedIn == true && x.JwtToken == user.JwtToken);
                if (_accessLog == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                //tokenHandler.ValidateToken(user.JwtToken, new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = new SymmetricSecurityKey(key),
                //    ValidateIssuer = false,
                //    ValidateAudience = false,
                //    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                //    ClockSkew = TimeSpan.Zero
                //}, out SecurityToken validatedToken);

                //var jwtToken = (JwtSecurityToken)validatedToken;
                //var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                //// return account id from JWT token if validation successful
                //return true;
            }
            catch
            {
                // return null if validation fails
                return false;
            }
        }
    }
}
