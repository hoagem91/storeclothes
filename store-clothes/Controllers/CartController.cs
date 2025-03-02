using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;

namespace store_clothes.Controllers
{
    public class CartController : Controller
    {
        private readonly storeclothesContext _context;

        public CartController(storeclothesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.Carts.Include(c => c.Product).ToListAsync();
            return View(cartItems);
        }
    }
}
