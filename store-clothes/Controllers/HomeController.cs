using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            var products = await _context.Products.ToListAsync();

            var model = new Tuple<IEnumerable<Category>, IEnumerable<Product>>(categories, products);
            return View(model);
        }
    }
}
