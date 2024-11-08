using GorevYonetimSistemi.Web.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}