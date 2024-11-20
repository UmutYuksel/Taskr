using System.Text.Json;
using GorevYonetimSistemi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class UserDutyController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5113/api/UserDuty";

        public UserDutyController
        (
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var userDutiesJson = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(userDutiesJson))
                {
                    var userDuties = JsonSerializer.Deserialize<IEnumerable<UserDuty>>(userDutiesJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (userDuties != null)
                    {
                        return View(userDuties);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No user tasks found.");
                    TempData["Error Message"] = "Tasks not found.";
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to fetch user tasks.");
            TempData["Error Message"] = "Failed to fetch user tasks.";
            return View();
        }
    }
}