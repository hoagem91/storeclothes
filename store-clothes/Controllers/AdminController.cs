using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Admin()
        {
            return View();
        }

        // Các action khác như Users, Settings, v.v.
        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
    }
}
