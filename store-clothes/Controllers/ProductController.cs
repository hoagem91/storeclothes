using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
