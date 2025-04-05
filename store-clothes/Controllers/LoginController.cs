using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using store_clothes.Models;
using System.Linq;

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

        // Xử lý đăng nhập (POST: /Login/Authenticate)
        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {
            // Tìm user trong database dựa trên email và password
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password); // Chưa băm mật khẩu

            if (user != null)
            {
                // Lưu thông tin user vào session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetInt32("UserId", user.Id); // Lưu UserId để dùng trong Cart/Favorites

                // Chuyển hướng về trang chủ sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }

            // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.Error = "Invalid email or password!";
            return View("Index");
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