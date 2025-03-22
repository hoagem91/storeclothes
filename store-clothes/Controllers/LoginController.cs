using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using store_clothes.Models;
using System.Linq;

namespace store_clothes.Controllers
{
    public class LoginController : Controller
    {
        private readonly storeclothesContext _context;

        public LoginController(storeclothesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password); // Không băm mật khẩu

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                return RedirectToAction("Index", "Home"); // Đăng nhập thành công
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index");
        }
    }
}
