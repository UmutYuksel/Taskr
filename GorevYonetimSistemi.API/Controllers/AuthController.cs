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
                var response = await _authService.RegisterAsync(registerDto);
                
                if (response == null)
                {
                    return BadRequest("Register unsuccessful");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);

                if (response == null)
                {
                    throw new KeyNotFoundException("Login unsuccessful");
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}