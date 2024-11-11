using GorevYonetimSistemi.Business.Dtos.User.Auth;
using Microsoft.AspNetCore.Http;

namespace GorevYonetimSistemi.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto);
        Task<bool> UserExistsAsync(string email);
    }
}