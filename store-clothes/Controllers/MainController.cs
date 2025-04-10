using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System.Threading.Tasks;
using System.Linq;

namespace store_clothes.Controllers
{
    public class MainController : Controller
    {
        private readonly storeclothesContext _context;

        public MainController(storeclothesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy tất cả sản phẩm từ database trước
            var allProducts = await _context.Products
                .OrderByDescending(p => p.Id) // Sắp xếp theo ID giảm dần (mới nhất)
                .ToListAsync();

            // Nhóm sản phẩm theo danh mục và chỉ lấy 4 sản phẩm mỗi nhóm
            var products = allProducts
                .GroupBy(p => p.CategoryId) // Nhóm theo danh mục
                .SelectMany(g => g.Take(5)) // Lấy 4 sản phẩm đầu tiên của mỗi nhóm
                .ToList();

            return View(products);
        }

    }
}
