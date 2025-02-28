using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
