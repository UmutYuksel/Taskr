using System.Text;
using System.Text.Json;
using GorevYonetimSistemi.Business.Dtos;
using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("User not authenticated");
            return View(registerDto);
        }

        public IActionResult Login()
        {
            var loginDto = new UserLoginDto();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("User not authenticated");
            return View(loginDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent
                (
                    JsonSerializer.Serialize(registerDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("http://localhost:5113/api/auth/register", jsonContent);

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
            if (ModelState.IsValid)
            {
                // API'ya kullanıcı bilgilerini gönder
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(loginDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("http://localhost:5113/api/auth/login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Token'ı alın
                    var token = await response.Content.ReadAsStringAsync();

                    // Cookie ayarlarını yap
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true, // JavaScript ile erişilememe
                        Secure = true,   // Sadece HTTPS üzerinden gönderilsin
                        Expires = DateTime.Now.AddHours(1)  // Token'ın geçerlilik süresi
                    };

                    // Token'ı cookie'ye ekle
                    Response.Cookies.Append("AuthToken", token, cookieOptions);

                    // Login işlemi başarılı, kullanıcının yönlendirilmesi
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    // Hatalı giriş bilgileri
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ModelState.AddModelError(string.Empty, "Giriş bilgileri geçersiz.");
                return View(loginDto);
            }

            // Hata durumunda tekrar login sayfasına dön
            return View(loginDto);
        }

        public async Task<IActionResult> Logout()
        {
            // Cookie'yi sil
            Response.Cookies.Delete("AuthToken");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}