using Microsoft.AspNetCore.Mvc;
using store_clothes.Attribute;
using store_clothes.Models; // Namespace của model User
using System.Linq;

public class UserController : Controller
{
    private readonly storeclothesContext _context; // Thay bằng DbContext của bạn

    public UserController(storeclothesContext context)
    {
        _context = context;
    }

    [ViewLayout("_AdminLayout")]
    public IActionResult Index()
    {
        var users = _context.Users.ToList(); // Lấy danh sách người dùng từ database
        return View(users); // Trả về view UserList.cshtml với model là danh sách users
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
        return RedirectToAction("Index", "User");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ViewLayout("_AdminLayout")]
    public ActionResult UpdateUser(User updatedUser)
    {
        var user = _context.Users.Find(updatedUser.Id);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy người dùng!";
            return RedirectToAction("Index");
        }

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

        _context.Users.Remove(user);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Xóa người dùng thành công!";
        return RedirectToAction("Index");
    }
}