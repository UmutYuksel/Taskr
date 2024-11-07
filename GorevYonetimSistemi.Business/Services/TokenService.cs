using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GorevYonetimSistemi.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly double _tokenExpiration;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"];
            _tokenExpiration = double.TryParse(configuration["Jwt:TokenExpirationHours"], out var expiration) ? expiration : 1;

        }

        public string GenerateJWToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Taskr",
                audience: "User",
                claims: claims,
                expires: DateTime.Now.AddHours(_tokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}