using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using store_clothes.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            string hashedPassword = HashPassword(password); // Mã hóa password trước khi so sánh

            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
