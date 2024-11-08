using System.Security.Claims;
using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJWToken(User user);
        ClaimsPrincipal ValidateToken(string token);
    }
}