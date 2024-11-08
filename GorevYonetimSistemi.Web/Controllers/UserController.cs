using GorevYonetimSistemi.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class UserController : Controller
    {

        private readonly UserApiService _userApiService;

        public UserController(UserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        public IActionResult Index()
        {
           if(User.Identity.IsAuthenticated)
           {
            Console.WriteLine("Kullanıcı var");
           } 
           Console.WriteLine("Kullanıcı yok");
            return View();
        }
    }
}
