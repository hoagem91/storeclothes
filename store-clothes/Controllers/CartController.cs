using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
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

        // Hiển thị giỏ hàng
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.Carts
                .Include(c => c.Product) // Lấy dữ liệu từ bảng Product
                .ToListAsync(); // Trả về danh sách Cart model đúng kiểu

            return View(cartItems); // Trả về View với danh sách Cart
        }


        // Cập nhật số lượng sản phẩm trong giỏ hàng
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
    }
}
