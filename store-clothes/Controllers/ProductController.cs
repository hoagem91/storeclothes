using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_clothes.Controllers
{
    public class ProductController : Controller
    {
        private readonly storeclothesContext _context;

        public ProductController(storeclothesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string size = null, string price = null, string search = null, int page = 1)
        {
            int pageSize = 10; // Số sản phẩm mỗi trang
            var productsQuery = _context.Products.AsQueryable();

            // Log để kiểm tra tham số nhận được
            Console.WriteLine($"Received parameters: size={size}, price={price}, search={search}, page={page}");

            // Lấy userId từ session
            int userId = HttpContext.Session.GetInt32("UserId") ?? 1;

            // Lấy danh sách productId đã được yêu thích
            var favoriteProductIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();

            // Lọc theo từ khóa tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower()));
                ViewBag.Search = search;
                Console.WriteLine("Applied search filter: " + search);
            }

            // Lọc theo kích cỡ (nếu có)
            if (!string.IsNullOrEmpty(size))
            {
                productsQuery = productsQuery.Where(p => p.Size != null && p.Size.Contains(size));
                ViewBag.SelectedSize = size;
                Console.WriteLine("Applied size filter: " + size);
            }

            // Lọc theo giá (nếu có)
            productsQuery = FilterByPrice(productsQuery, price);
            if (!string.IsNullOrEmpty(price))
            {
                ViewBag.SelectedPrice = price;
                Console.WriteLine("Applied price filter: " + price);
            }

            // Phân trang
            int totalItems = await productsQuery.CountAsync();
            Console.WriteLine($"Total items after filtering: {totalItems}");
            var products = await productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy danh sách kích cỡ
            var sizes = await _context.Products
                .Where(p => !string.IsNullOrEmpty(p.Size))
                .Select(p => p.Size)
                .ToListAsync();

            // Tách các kích cỡ thành danh sách riêng lẻ (S, M, L, XL, XXL)
            var distinctSizes = sizes
                .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim()) // Loại bỏ khoảng trắng thừa
                .Distinct(StringComparer.OrdinalIgnoreCase) // Loại bỏ trùng lặp, không phân biệt hoa thường
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase) // Sắp xếp không phân biệt hoa thường
                .Select(s => new { Id = s, Name = s })
                .ToList();

            var sizeOptionsAsObjects = distinctSizes.Cast<object>().ToList();

            var priceRanges = new List<object>
            {
                new { Id = "0-100000", Name = "Dưới 100.000đ" },
                new { Id = "100000-200000", Name = "100.000 - 200.000đ" },
                new { Id = "200000-300000", Name = "200.000 - 300.000đ" },
                new { Id = "above-300000", Name = "Trên 300.000đ" }
            };

            // Truyền dữ liệu vào ViewBag
            ViewBag.Sizes = sizeOptionsAsObjects ?? new List<object>();
            ViewBag.PriceRanges = priceRanges;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.FavoriteProductIds = favoriteProductIds; // Truyền danh sách productId đã yêu thích

            // Lấy số lượng sản phẩm trong giỏ hàng từ session
            ViewBag.CartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            // Lấy số lượng sản phẩm trong giỏ hàng từ session
            ViewBag.CartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterData()
        {
            var sizes = await _context.Products
                .Where(p => !string.IsNullOrEmpty(p.Size))
                .Select(p => p.Size)
                .ToListAsync();

            // Tách các kích cỡ thành danh sách riêng lẻ (S, M, L, XL, XXL)
            var distinctSizes = sizes
                .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim()) // Loại bỏ khoảng trắng thừa
                .Distinct(StringComparer.OrdinalIgnoreCase) // Loại bỏ trùng lặp, không phân biệt hoa thường
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase) // Sắp xếp không phân biệt hoa thường
                .Select(s => new { Id = s, Name = s })
                .ToList();

            var priceRanges = new List<object>
            {
                new { Id = "0-100000", Name = "Dưới 100.000đ" },
                new { Id = "100000-200000", Name = "100.000 - 200.000đ" },
                new { Id = "200000-300000", Name = "200.000 - 300.000đ" },
                new { Id = "above-300000", Name = "Trên 300.000đ" }
            };

            return Json(new { sizes = distinctSizes, priceRanges });
        }

        private IQueryable<Product> FilterByPrice(IQueryable<Product> query, string price)
        {
            if (string.IsNullOrEmpty(price)) return query;

            switch (price)
            {
                case "0-100000":
                    return query.Where(p => p.Price <= 100000);
                case "100000-200000":
                    return query.Where(p => p.Price > 100000 && p.Price <= 200000);
                case "200000-300000":
                    return query.Where(p => p.Price > 200000 && p.Price <= 300000);
                case "above-300000":
                    return query.Where(p => p.Price > 300000);
                default:
                    return query;
            }
        }
    }
}