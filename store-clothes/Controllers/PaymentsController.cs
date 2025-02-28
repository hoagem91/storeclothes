using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class PaymentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
