using GorevYonetimSistemi.Business.Dtos.Duty;
using GorevYonetimSistemi.Core.Extension;
using GorevYonetimSistemi.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class DutyController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5113/api/duty";

        public DutyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var dutiesJson = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(dutiesJson))
                {
                    // JSON'dan verileri al
                    var duties = JsonSerializer.Deserialize<IEnumerable<DutyDto>>(dutiesJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Eğer duties null değilse, sıralama işlemini yapalım
                    if (duties != null)
                    {
                        // CreatedDate'e göre sıralama (en yeni en üstte)
                        var sortedDuties = duties
                            .OrderByDescending(d => d.CreatedDate) // En yeni tarihe göre sıralama
                            .Select(d => new DutyViewModel
                            {
                                DutyId = d.DutyId,
                                Title = d.Title,
                                Description = d.Description,
                                Progress = d.Progress,
                                ProgressDisplayName = d.Progress.GetDisplayName(), // Enum Display Name
                                CreatedDate = d.CreatedDate
                            });

                        return View(sortedDuties);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No duties found.");
                    TempData["Error Message"] = "Tasks not found.";
                    return View();
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to fetch duties.");
            TempData["Error Message"] = "Failed to fetch tasks.";
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new DutyDto();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DutyDto dutyDto)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(dutyDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiBaseUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error Message"] = "An error occurred while creating the task.";
                ModelState.AddModelError(string.Empty, "An error occurred while creating the task.");
            }
            TempData["Error Message"] = "An error occurred while creating the task.";
            return View(dutyDto);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid dutyId, DutyDto dutyDto)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(dutyDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{dutyId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Task updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error Message"] = "An error occurred while updating the task.";
                ModelState.AddModelError(string.Empty, "An error occurred while updating the task.");
            }
            TempData["Error Message"] = "Please complete requied areas.";
            return View(dutyDto);
        }

        // Görev silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid dutyId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{dutyId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Task deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error Message"] = "Task could not be deleted."; ;
            return View();
        }
    }
}
