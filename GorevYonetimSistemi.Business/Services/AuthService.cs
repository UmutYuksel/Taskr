using AutoMapper;
using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Business.Services.Interfaces;
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
        private readonly IMapper _mapper; 

        public AuthService
        (
            IUserRepository userRepository,
            PasswordService passwordService,
            ITokenService tokenService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto registerDto)
        {
            Console.WriteLine($"[DEBUG] Checking if user exists for email: {registerDto.Email}");

            var existingUser = await _userRepository.GetUserByEmail(registerDto.Email!);

            if(existingUser != null)
            {
                Console.WriteLine($"[DEBUG] User already exists with email: {registerDto.Email}");
                throw new KeyNotFoundException("User already exists");
            }

            Console.WriteLine($"[DEBUG] Creating new user with email: {registerDto.Email}");
            var hashesPassword = _passwordService.HashPassword(registerDto.Password!);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = registerDto.Email,
                Password = hashesPassword,
                Username = registerDto.Username,
                Role = Role.User
            };

            await _userRepository.CreateUserAsync(user);

            Console.WriteLine($"[DEBUG] User created successfully with email: {registerDto.Email}");

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
            Console.WriteLine($"[DEBUG] Attempting login with email: {loginDto.Email}");

            var user = await _userRepository.GetUserByEmail(loginDto.Email!);

            if (user == null || !_passwordService.VerifyPassword(loginDto.Password!, user.Password!))
            {
                Console.WriteLine($"[DEBUG] Login failed for email: {loginDto.Email}");
                throw new Exception("Invalid email or password");
            }

            Console.WriteLine($"[DEBUG] Login successful for email: {loginDto.Email}");

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