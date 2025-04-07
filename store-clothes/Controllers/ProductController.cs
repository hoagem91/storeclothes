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

        [ViewLayout("_AdminLayout")]
        public IActionResult ProductAdmin()
        {
            var products = _context.Products.ToList(); // Lấy danh sách sản phẩm từ database
            return View(products); // Trả về view Index.cshtml trong /Views/Product/
        }

        [HttpGet]
        [ViewLayout("_AdminLayout")]
        public IActionResult CreateProduct()
        {
            // Populate the categories for the dropdown
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        // hiển thị form UpdateProduct
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
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
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

        // phần thêm sửa xóa Admin
        // POST: Xử lý thêm sản phẩm với ảnh
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile ImageFile)
        {
            Console.WriteLine("Received Product: " + System.Text.Json.JsonSerializer.Serialize(product));
            Console.WriteLine("ImageFile: " + (ImageFile != null ? ImageFile.FileName : "null"));

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            // Kiểm tra trùng lặp sản phẩm
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower());

            if (existingProduct != null)
            {
                Console.WriteLine("Duplicate product found: " + existingProduct.Name);
                ModelState.AddModelError("Name", "Sản phẩm đã tồn tại!");
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            // Xử lý lưu ảnh nếu có
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Đảm bảo rằng CategoryId không null
                if (!product.CategoryId.HasValue)
                {
                    Console.WriteLine("CategoryId is null");
                    ModelState.AddModelError("CategoryId", "Vui lòng chọn danh mục cho sản phẩm!");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                // Lấy thông tin danh mục từ CategoryId
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

                if (category == null)
                {
                    Console.WriteLine($"Category not found for CategoryId: {product.CategoryId}");
                    ModelState.AddModelError("CategoryId", "Danh mục không hợp lệ!");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                // Chuẩn hóa tên danh mục để khớp với tên thư mục
                string categoryFolder = category.Name.ToLower().Trim();
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assests", "products", categoryFolder);

                try
                {
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
                catch (Exception ex)
                {
                    Console.WriteLine("File save error: " + ex.Message);
                    ModelState.AddModelError("ImageFile", $"Lỗi khi lưu ảnh: {ex.Message}");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(ProductAdmin));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Database error: " + (ex.InnerException?.Message ?? ex.Message));
                ModelState.AddModelError("", $"Lỗi khi lưu sản phẩm: {ex.InnerException?.Message ?? ex.Message}");
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ViewLayout("_AdminLayout")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products
                           .Where(p => p.Id != id)
                           .FirstOrDefaultAsync(p => p.Name == product.Name);
                    if (existingProduct != null)
                    {
                        ModelState.AddModelError("Name", "Sản phẩm này đã tồn tại !");
                        return View(product);
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa sản phẩm thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductAdmin));
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        // Xóa sản phẩm
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
    }
}