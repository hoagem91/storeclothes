using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;

namespace store_clothes.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly storeclothesContext _context;

        public FavoritesController(storeclothesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            var favorites = _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == userId.Value)
                .ToList();
            return View(favorites);
        }

        [HttpPost]
        public IActionResult AddToFavorites(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            if (productId <= 0)
            {
                return Json(new { success = false, message = "ID sản phẩm không hợp lệ!" });
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            if (_context.Favorites.Any(f => f.UserId == userId.Value && f.ProductId == productId))
            {
                return Json(new { success = false, message = "Sản phẩm đã có trong danh sách yêu thích!" });
            }

            var favorite = new Favorite
            {
                UserId = userId.Value,
                ProductId = productId,
                CreatedAt = DateTime.Now
            };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã thêm vào danh sách yêu thích!" });
        }

        [HttpPost]
        public IActionResult RemoveFromFavorites(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var favorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId.Value && f.ProductId == productId);

            if (favorite == null)
            {
                return Json(new { success = false, message = "Sản phẩm không có trong danh sách yêu thích!" });
            }

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã xóa khỏi danh sách yêu thích!" });
        }
    }
}