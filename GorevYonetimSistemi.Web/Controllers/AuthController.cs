using System.Text;
using GorevYonetimSistemi.Business.Dtos.User.Auth;
using GorevYonetimSistemi.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                return RedirectToAction("Index", "User");
            }
            return View(registerDto);
        }

        public IActionResult Login()
        {
            var loginDto = new UserLoginDto();

            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "User");
            }
            return View(loginDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent
                (
                    JsonConvert.SerializeObject(registerDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var jsonString = JsonConvert.SerializeObject(registerDto);
                Console.WriteLine($"Serialized JSON: {jsonString}");


                // API çağrısı
                var response = await _httpClient.PostAsync("http://localhost:5113/api/auth/register", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    // API'dan gelen hata mesajını al
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Registration failed: {errorMessage}");
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            }

            return View(registerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent
                (
                    JsonConvert.SerializeObject(loginDto), // Using Newtonsoft.Json
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("http://localhost:5113/api/auth/login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // API yanıtını okuma
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                    // Eğer deserialization başarılıysa
                    if (tokenResponse != null)
                    {
                        // Token'ı ve diğer bilgileri alıyoruz
                        var token = tokenResponse.Token;

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false,
                            Expires = DateTime.Now.AddHours(1)
                        };

                        Response.Cookies.Append("AuthToken", token!, cookieOptions);

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        // Token alınamadıysa hata mesajı ekle
                        ModelState.AddModelError(string.Empty, "Invalid token received from the server.");
                    }
                }
                else
                {
                    // Hatalı giriş bilgileri
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Login failed: {errorMessage}");
                }
            }
            else
            {
                // Model geçerli değilse hata mesajı
                ModelState.AddModelError(string.Empty, "Giriş bilgileri geçersiz.");
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