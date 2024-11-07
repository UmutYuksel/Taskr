using GorevYonetimSistemi.Data.Entities;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJWToken(User user);
    }
}