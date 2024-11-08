using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Register method called with email: {registerDto.Email}");

                var response = await _authService.RegisterAsync(registerDto);
                
                if (response == null)
                {
                    Console.WriteLine($"[DEBUG] Registration failed for email: {registerDto.Email}");
                    return BadRequest("Register unsuccessful");
                }

                Console.WriteLine($"[DEBUG] Registration successful for email: {registerDto.Email}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error occurred during registration: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Login method called with email: {loginDto.Email}");

                var response = await _authService.LoginAsync(loginDto);

                if (response == null)
                {
                    Console.WriteLine($"[DEBUG] Login unsuccessful for email: {loginDto.Email}");
                    throw new KeyNotFoundException("Login unsuccessful");
                }

                Console.WriteLine($"[DEBUG] Login successful for email: {loginDto.Email}");
                return Ok(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] Error occurred during login for email: {loginDto.Email}: {ex.Message}");
                return Unauthorized(ex.Message);
            }
        }
    }
}
