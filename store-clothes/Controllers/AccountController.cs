using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace store_clothes.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Handle login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Giả lập xác thực đơn giản
            if (username == "admin" && password == "123")
            {
                // Lưu thông tin vào Session
                HttpContext.Session.SetString("UserName", "Admin");
                HttpContext.Session.SetString("UserEmail", "admin@example.com");

                return RedirectToAction("Info");
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return View();
        }

        // GET: Account Info page
        public IActionResult Info()
        {
            // Nếu chưa đăng nhập thì chuyển về Login
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index","Login");
            }

            // Chặn trình duyệt lưu cache để không quay lại được sau khi logout
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            // Truyền thông tin user
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }

        // POST: Logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa toàn bộ session
            HttpContext.Session.Clear();

            return RedirectToAction("Index","Login");
        }
    }
}
