using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System.Threading.Tasks;

namespace store_clothes.Controllers
{
    public class HomeController : Controller
    {
        private readonly storeclothesContext _context;

        public HomeController(storeclothesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
    }
}
