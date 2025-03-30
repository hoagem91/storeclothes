using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
