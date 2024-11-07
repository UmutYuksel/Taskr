using GorevYonetimSistemi.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserApiService _userApiService;
        private readonly DutyApiService _dutyApiService;

        public AdminController(UserApiService userApiService, DutyApiService dutyApiService)
        {
            _userApiService = userApiService;
            _dutyApiService = dutyApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}