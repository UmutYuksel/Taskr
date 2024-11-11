using Microsoft.AspNetCore.Mvc;

namespace GorevYonetimSistemi.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
           if(User.Identity!.IsAuthenticated)
           {
            Console.WriteLine("Kullanıcı var");
           } 
           Console.WriteLine("Kullanıcı yok");
            return View();
        }
    }
}
