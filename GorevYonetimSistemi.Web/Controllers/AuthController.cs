using System.Text;
using System.Text.Json;
using GorevYonetimSistemi.Business.Dtos;
using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerDto = new UserRegisterDto();
            
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View(registerDto);
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if(ModelState.IsValid)
            {
                var jsonContent = new StringContent
                (
                    JsonSerializer.Serialize(registerDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("https://localhost:5113/api/auth/register", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");                    
                }
            }
            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");   
            return View(registerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            if(ModelState.IsValid)
            {
                var jsonContent = new StringContent
                (
                    JsonSerializer.Serialize(loginDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("https://localhost:5113/api/auth/login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    HttpContext.Session.SetString("AuthToken",token);

                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            return View(loginDto);
        }

    }
}