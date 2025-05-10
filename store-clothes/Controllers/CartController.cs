using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System.Linq;
using System.Threading.Tasks;

namespace store_clothes.Controllers
{
    public class CartController : Controller
    {
        private readonly storeclothesContext _context;

        public CartController(storeclothesContext context)
        {
            _context = context;
        }

        // Hiển thị trang giỏ hàng
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Index", "Login"); // Chưa đăng nhập
            }

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId.Value)
                .ToListAsync();

            return View(cartItems);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, string selectedSize)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng!" });
                }

                // Kiểm tra productId hợp lệ
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = $"Sản phẩm với ID {productId} không tồn tại!" });
                }

                // Kiểm tra quantity hợp lệ
                if (quantity <= 0)
                {
                    return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
                }

                // Kiểm tra selectedSize
                if (string.IsNullOrEmpty(selectedSize))
                {
                    return Json(new { success = false, message = "Vui lòng chọn kích cỡ!" });
                }

                // Kiểm tra sản phẩm đã có trong giỏ hàng chưa (cùng kích cỡ)
                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId.Value && c.ProductId == productId && c.Size == selectedSize);

                if (cartItem != null)
                {
                    // Nếu đã có, tăng số lượng
                    cartItem.Quantity += quantity;
                }
                else
                {
                    // Nếu chưa có, thêm mới
                    cartItem = new Cart
                    {
                        UserId = userId.Value,
                        ProductId = productId,
                        Quantity = quantity,
                        Size = selectedSize
                    };
                    _context.Carts.Add(cartItem);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.Id == cartId && c.UserId == userId.Value);
                if (cartItem == null)
                {
                    return Json(new { success = false, message = $"Mục giỏ hàng với ID {cartId} không tồn tại hoặc không thuộc về bạn!" });
                }

                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        // Lấy danh sách sản phẩm trong Mini Cart
        [HttpGet]
        public async Task<IActionResult> GetMiniCartItems()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId.Value)
                .Select(c => new
                {
                    id = c.Id,
                    productId = c.ProductId,
                    name = c.Product.Name,
                    price = c.Product.Price.HasValue
                        ? string.Format("{0:N0} đ", c.Product.Price.Value)
                        : "0 đ",
                    priceValue = c.Product.Price ?? 0,
                    quantity = c.Quantity,
                    size = c.Size,
                    imageUrl = c.Product.ImageUrl != null ? $"/assests/products/{c.Product.ImageUrl}" : "/images/default-product.jpg"
                })
                .ToListAsync();

            return Json(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng!" });
                }

                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();

                // Cập nhật số lượng sản phẩm trong giỏ hàng
                var cartCount = await _context.Carts
                    .Where(c => c.UserId == userId.Value)
                    .SumAsync(c => c.Quantity);
                HttpContext.Session.SetInt32("CartCount", cartCount);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập!" });
                }

                var userCart = await _context.Carts
                    .Where(c => c.UserId == userId.Value)
                    .ToListAsync();

                if (userCart.Any())
                {
                    _context.Carts.RemoveRange(userCart);
                    await _context.SaveChangesAsync();
                    
                    // Reset cart count in session
                    HttpContext.Session.SetInt32("CartCount", 0);
                    
                    return Json(new { success = true });
                }

                return Json(new { success = true, message = "Giỏ hàng đã trống" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}