using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Attribute; // Namespace của ViewLayoutAttribute
using store_clothes.Models;

namespace store_clothes.Controllers
{
    // Controller này sẽ hỗ trợ cả API và View
    public class CategoryController : Controller // Kế thừa từ Controller thay vì ControllerBase
    {
        private readonly storeclothesContext _context;

        public CategoryController(storeclothesContext context)
        {
            _context = context;
        }

        // Action để hiển thị view danh sách danh mục
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync(); // Lấy danh sách danh mục
            return View(categories); // Trả về view Index.cshtml với model là danh sách danh mục
        }

        // API: Lấy danh sách tất cả Categories
        [HttpGet]
        [Route("api/Category/GetCategories")] // Đảm bảo route không xung đột
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // API: Lấy 1 Category theo ID
        [HttpGet]
        [Route("api/Category/GetCategory/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }
    }
}