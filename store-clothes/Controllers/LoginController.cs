using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using store_clothes.Models;
using System.Linq;
using Org.BouncyCastle.Crypto.Generators;
using store_clothes.Attribute;

namespace store_clothes.Controllers
{
    public class LoginController : Controller
    {
        private readonly storeclothesContext _context;

        // Constructor nhận storeclothesContext qua Dependency Injection
        public LoginController(storeclothesContext context)
        {
            _context = context;
        }

        // Hiển thị trang đăng nhập (GET: /Login)
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoginAdmin()
        {
            return View();
        }
        // Xử lý đăng nhập (POST: /Login/Authenticate)
        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password); // Không băm mật khẩu

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Home"); // Đăng nhập thành công
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }

        [HttpPost]
        [ViewLayout("_AdminLayout")]
        public IActionResult AuthenticateAdmin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and Password are required!";
                return View("LoginAdmin");
            }

            // Tìm admin theo username
            var admin = _context.Admins.FirstOrDefault(a => a.Name == username);

            if (admin == null || admin.Password != password)  // So sánh trực tiếp
            {
                ViewBag.Error = "Invalid username or password!";
                return View("LoginAdmin");
            }

            // Lưu thông tin vào session
            HttpContext.Session.SetString("AdminName", admin.Name);
            HttpContext.Session.SetInt32("AdminId", admin.Id);
            HttpContext.Session.SetString("UserRole", "Admin");

            return RedirectToAction("Index", "User");
        }

        // Xử lý đăng xuất (GET: /Login/Logout)
        public IActionResult Logout()
        {
            // Xóa toàn bộ session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Index");
        }
    }
}