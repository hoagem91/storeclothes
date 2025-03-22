using Microsoft.AspNetCore.Mvc;
using store_clothes.Models;
using System.Linq;

namespace store_clothes.Controllers
{
    public class RegisterController : Controller
    {
        private readonly storeclothesContext _context; // Inject DbContext

        public RegisterController(storeclothesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", user);
            }

            // Kiểm tra email đã tồn tại chưa
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Email đã tồn tại!";
                return RedirectToAction("Index");
            }

            // Lưu user vào database (KHÔNG mã hóa mật khẩu)
            _context.Users.Add(user);
            _context.SaveChanges();

            // Lưu thông báo thành công
            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";

            // Chuyển hướng về trang Login
            return RedirectToAction("Index", "Login");
        }
    }
}
