using AutoMapper;
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
    public class JwtService : IJwtService
    {
        private readonly ProjectPosDbContext _db;
        private readonly IMapper _mapper;
        public IConfiguration _configuration;


        public JwtService(ProjectPosDbContext dbContext, IConfiguration config, IMapper mapper)
        {
            _db = dbContext;
            _configuration = config;
            _mapper = mapper;
        }


        public string GenerateJwtToken(UserDto user)
        {
            //var claims = new[] 
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            //    new Claim("Id", user.Id.ToString()),
            //    new Claim("Password", user.Password),
            //    new Claim("IsActive", user.IsActive.ToString()),
            //    new Claim("Email", user.Email),
            //    new Claim("Role", user.RoleId.ToString())
            //};

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(
            //    _configuration["Jwt:Issuer"],
            //    _configuration["Jwt:Audience"],
            //    claims,
            //    expires: DateTime.UtcNow.AddMinutes(10),
            //    signingCredentials: signIn);

            //return new JwtSecurityTokenHandler().WriteToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserName", user.UserName) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
