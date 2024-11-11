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
        public async Task<IActionResult> RegisterUser([FromForm] UserRegisterDto userRegisterDto)
        {
            try
            {
                // Kayıt işlemini gerçekleştirin
                var authResponse = await _authService.RegisterAsync(userRegisterDto);

                // Başarılı yanıt döndür
                return Ok(authResponse);
            }
            catch (KeyNotFoundException ex)
            {
                // Kullanıcı zaten var
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Diğer hatalar
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error occurred during login for email: {loginDto.Email}: {ex.Message}");
                return Unauthorized(ex.Message);
            }
        }
    }
}
