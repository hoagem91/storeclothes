using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Attribute;
using store_clothes.Models; // Namespace của model User và Cart
using System.Linq;

namespace store_clothes.Controllers
{
    public class UserController : Controller
    {
        private readonly storeclothesContext _context;

        public UserController(storeclothesContext context)
        {
            _context = context;
        }

        [ViewLayout("_AdminLayout")]
        public IActionResult Index()
        {
            var users = _context.Users.ToList(); // Lấy danh sách người dùng
            return View(users);
        }

        [ViewLayout("_AdminLayout")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [ViewLayout("_AdminLayout")]
        public ActionResult UpdateUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [ViewLayout("_AdminLayout")]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        [ViewLayout("_AdminLayout")]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUser", user); // Trả về view CreateUser thay vì Index
            }

            // Kiểm tra email đã tồn tại
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Email đã tồn tại!";
                return View("CreateUser", user);
            }

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công!";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo người dùng: {ex.InnerException?.Message ?? ex.Message}";
                return View("CreateUser", user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public ActionResult UpdateUser(User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            var user = _context.Users.Find(updatedUser.Id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index");
            }

            // Kiểm tra email trùng lặp
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == updatedUser.Email && u.Id != updatedUser.Id);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Email đã tồn tại!";
                return View(updatedUser);
            }

            try
            {
                // Cập nhật thông tin
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;

                // Kiểm tra nếu nhập mật khẩu mới
                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật người dùng: {ex.InnerException?.Message ?? ex.Message}";
                return View(updatedUser);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
                return RedirectToAction("Index");
            }

            try
            {
                // Xóa các bản ghi trong bảng cart liên quan đến user
                var userCarts = _context.Carts.Where(c => c.UserId == id).ToList();
                _context.Carts.RemoveRange(userCarts);

                // Xóa user
                _context.Users.Remove(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa người dùng: {ex.InnerException?.Message ?? ex.Message}";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}