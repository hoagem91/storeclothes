using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;

namespace store_clothes.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly storeclothesContext _context;

        public PaymentsController(storeclothesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments.ToListAsync();
            return View(payments);
        }
    }
}
