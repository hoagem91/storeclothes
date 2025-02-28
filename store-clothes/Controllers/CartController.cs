using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
