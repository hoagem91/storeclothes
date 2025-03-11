using Microsoft.AspNetCore.Mvc;

namespace store_clothes.Controllers
{
    public class RegisterController : Controller
    {
        // Hiển thị trang đăng ký (Register)
        public IActionResult Index()
        {
            return View(); // Tìm file Views/Register/Index.cshtml
        }

        // Chuyển hướng đến trang đăng nhập (Login)
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Login"); // Điều hướng đến LoginController.Index()
        }
    }
}

