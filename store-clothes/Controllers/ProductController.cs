using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Attribute;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(storeclothesContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            var distinctSizes = sizes
                .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
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
            ViewBag.FavoriteProductIds = favoriteProductIds;
            ViewBag.CartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

            return View(products);
        }

        private IQueryable<Product> FilterByPrice(IQueryable<Product> query, string price)
        {
            Console.WriteLine($"Filtering by price: {price}");
            if (string.IsNullOrEmpty(price)) return query;

            switch (price)
            {
                case "0-100000":
                    return query.Where(p => p.Price.HasValue && p.Price.Value <= 100000);
                case "100000-200000":
                    return query.Where(p => p.Price.HasValue && p.Price.Value > 100000 && p.Price.Value <= 200000);
                case "200000-300000":
                    return query.Where(p => p.Price.HasValue && p.Price.Value > 200000 && p.Price.Value <= 300000);
                case "above-300000":
                    return query.Where(p => p.Price.HasValue && p.Price.Value > 300000);
                default:
                    Console.WriteLine($"Unknown price range: {price}");
                    return query;
            }
        }

        // Các action khác giữ nguyên
        [ViewLayout("_AdminLayout")]
        public IActionResult ProductAdmin()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        [ViewLayout("_AdminLayout")]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpGet]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }
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

            var distinctSizes = sizes
                .SelectMany(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower());

            if (existingProduct != null)
            {
                ModelState.AddModelError("Name", "Sản phẩm đã tồn tại!");
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!product.CategoryId.HasValue)
                {
                    ModelState.AddModelError("CategoryId", "Vui lòng chọn danh mục!");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Danh mục không hợp lệ!");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                string categoryFolder = category.Name.ToLower().Trim();
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assests", "products", categoryFolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Path.GetFileNameWithoutExtension(ImageFile.FileName)}{Path.GetExtension(ImageFile.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                product.ImageUrl = $"{categoryFolder}/{uniqueFileName}";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(ProductAdmin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products
                    .Where(p => p.Id != id)
                    .FirstOrDefaultAsync(p => p.Name == product.Name);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("Name", "Sản phẩm này đã tồn tại!");
                    return View(product);
                }
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sửa sản phẩm thành công";
                return RedirectToAction(nameof(ProductAdmin));
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction(nameof(ProductAdmin));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}