using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Core.Enums;
using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace GorevYonetimSistemi.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService; 
        private readonly ITokenService _tokenService; 

        public AuthService
        (
            IUserRepository userRepository,
            PasswordService passwordService,
            ITokenService tokenService
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {

            var existingUser = await _userRepository.GetUserByEmail(registerDto.Email!, throwIfNotFound: false);

            if(existingUser != null)
            {
                throw new KeyNotFoundException("User already exists");
            }

            var hashesPassword = _passwordService.HashPassword(registerDto.Password!);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = registerDto.Email,
                Password = hashesPassword,
                Username = registerDto.Username,
                Role = RoleEnum.User
            };

            await _userRepository.CreateUserAsync(user);
            var token = _tokenService.GenerateJWToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email,
                Message = "Registration successful"
            };
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.Email!); // Hata fırlatır

            if (user == null || !_passwordService.VerifyPassword(loginDto.Password!, user.Password!))
            {
                throw new Exception("Invalid email or password");
            }

            var token = _tokenService.GenerateJWToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email,
                Message = "Login successful"
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user != null;
        }
    }
}