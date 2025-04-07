﻿using Microsoft.AspNetCore.Mvc;
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
            // Kiểm tra trong bảng Users
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", "User");

                return RedirectToAction("Index", "Home"); // Điều hướng User về Home
            }

            // Kiểm tra trong bảng Admins
            var admin = _context.Admins.FirstOrDefault(a => a.name == email); // Đổi name thành Email

            if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.password))
            {
                HttpContext.Session.SetString("AdminName", admin.name);
                HttpContext.Session.SetInt32("AdminId", admin.id);
                HttpContext.Session.SetString("UserRole", "Admin");

                return RedirectToAction("Admin", "Admin"); // Điều hướng Admin về Admin Panel
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
            var admin = _context.Admins.FirstOrDefault(a => a.name == username);

            if (admin == null || admin.password != password)  // So sánh trực tiếp
            {
                ViewBag.Error = "Invalid username or password!";
                return View("LoginAdmin");
            }

            // Lưu thông tin vào session
            HttpContext.Session.SetString("AdminName", admin.name);
            HttpContext.Session.SetInt32("AdminId", admin.id);
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