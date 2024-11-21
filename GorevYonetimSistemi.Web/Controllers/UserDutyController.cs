using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GorevYonetimSistemi.Business.Dtos;
using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Business.Dtos.User;
using GorevYonetimSistemi.Business.Dtos.UserDuty;
using GorevYonetimSistemi.Core.Extension;
using GorevYonetimSistemi.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    // UserDutyDto tipinde veriyi direkt deserialize et
                    var userDuties = JsonSerializer.Deserialize<List<UserDutyDto>>(userDutiesJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve // Döngüsel referanslar için
                    });

                    if (userDuties != null)
                    {
                        var sortedUserDuties = userDuties
                            .OrderByDescending(u => u.DutyCreatedDate)
                            .Select(u => new UserDutyViewModel
                            {
                                UserDutyId = u.UserDutyId,
                                DutyId = u.DutyId,
                                DutyTitle = u.DutyTitle,
                                DutyDescription = u.DutyDescription,
                                DutyProgress = u.DutyProgress,
                                DutyProgressDisplay = u.DutyProgress.GetDisplayName(),
                                DutyCreatedDate = u.DutyCreatedDate,
                                Users = u.Users
                            });

                        return View(sortedUserDuties);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No user duties found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Tasks not found.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to fetch user tasks.";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userResponse = await _httpClient.GetAsync("http://localhost:5113/api/User");
            var dutyResponse = await _httpClient.GetAsync("http://localhost:5113/api/Duty");

            if (userResponse.IsSuccessStatusCode && dutyResponse.IsSuccessStatusCode)
            {
                var usersJson = await userResponse.Content.ReadAsStringAsync();
                var dutiesJson = await dutyResponse.Content.ReadAsStringAsync();

                Console.WriteLine(usersJson);
                Console.WriteLine(dutiesJson);

                var users = JsonSerializer.Deserialize<List<UserDto>>(usersJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var duties = JsonSerializer.Deserialize<List<DutyDto>>(dutiesJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var viewModel = new UserDutyAssignViewModel
                {
                    Users = users,
                    Duties = duties,
                    UserDutyDto = new UserDutyDto()
                };
                if (users == null || !users.Any())
                {
                    TempData["ErrorMessage"] = "No users found.";
                }

                if (duties == null || !duties.Any())
                {
                    TempData["ErrorMessage"] = "No duties found.";
                }

                ViewBag.UsersList = new SelectList(users, "UserId", "Username");
                ViewBag.DutiesList = new SelectList(duties, "DutyId", "DutyTitle");
                return View(viewModel);
            }
            TempData["ErrorMessage"] = "Failed to load users or duties.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDutyAssignViewModel viewModel)
        {
            ModelState.Remove(nameof(viewModel.Users));
            ModelState.Remove(nameof(viewModel.Duties));
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        viewModel.UserDutyDto.UserId,
                        viewModel.UserDutyDto.DutyId
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/{viewModel.UserDutyDto.UserId}/duties/{viewModel.UserDutyDto.DutyId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Assign created successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error Message"] = "An error occurred while createing the assign.";
                ModelState.AddModelError(string.Empty, "An error occurred while creating the assign.");
            }
            TempData["Error Message"] = "Please complete requied areas.";
            return View(viewModel);
        }
    }
}