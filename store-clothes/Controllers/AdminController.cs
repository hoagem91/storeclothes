using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult LoginAdmin()
        {
            return View();
        }

        // Các action khác như Users, Settings, v.v.
        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}
