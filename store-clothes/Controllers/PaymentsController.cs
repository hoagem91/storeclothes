using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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
            List<CartItemInputModel> clientCartItems = null;
        
            if (TempData["CartData"] != null)
            {
                var json = TempData["CartData"].ToString();
                clientCartItems = JsonSerializer.Deserialize<List<CartItemInputModel>>(json);
            }

            if (clientCartItems == null || !clientCartItems.Any())
                return RedirectToAction("EmptyCart");

            var model = new PaymentModel
            {
                CartItems = clientCartItems.Select(c => new CartItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Product = new Product
                    {
                        Id = c.ProductId,
                        Name = c.Name,
                        Price = c.PriceValue,
                        ImageUrl = c.ImageUrl
                    }
                }).ToList(),
                FullName = "",
                Address = "",
                Phone = "",
                PaymentMethod = "COD"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaymentModel model)
        {
            if (!ModelState.IsValid || model.CartItems == null || !model.CartItems.Any())
            {
                return View(model);
            }

            var orderId = await SaveOrder(model);
            await SavePayment(orderId, model.PaymentMethod);

            return RedirectToAction("Success");
        }

        [HttpPost]
        public IActionResult StartCheckout([FromBody] List<CartItemInputModel> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                return BadRequest("Giỏ hàng trống.");

            TempData["CartData"] = JsonSerializer.Serialize(cartItems);
            Console.WriteLine(cartItems);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int userId, int productId, int quantity, decimal totalPrice, string paymentMethod)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0 || totalPrice <= 0 || string.IsNullOrEmpty(paymentMethod))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var order = new Order
            {
                UserId = userId,
                TotalPrice = totalPrice,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItem = new OrdersItem
            {
                OrderId = order.Id,
                ProductId = productId,
                Quantity = quantity,
                Price = totalPrice / quantity
            };

            _context.OrdersItems.Add(orderItem);

            var payment = new Payment
            {
                UserId = userId,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Completed",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }

        public async Task<IActionResult> Success()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                // Nếu userId là null hoặc rỗng, chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Chuyển userIdString sang int
            if (!int.TryParse(userIdString, out int userId))
            {
                // Nếu không thể chuyển đổi, chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Lấy thông tin thanh toán gần nhất cho người dùng
            var latestPayment = await _context.Payments
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync();

            if (latestPayment == null)
            {
                // Nếu không tìm thấy thanh toán gần nhất, chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Chỉ hiển thị thông báo thanh toán thành công
            var viewModel = new PaymentSuccessViewModel
            {
                TransactionId = latestPayment.TransactionId ?? "N/A", // Nếu không có TransactionId, dùng "N/A"
                PaymentDate = latestPayment.PaymentDate, // Không cần kiểm tra nullable nữa vì PaymentDate đã có giá trị
                EstimatedDeliveryDate = DateTime.Now.AddDays(3) // Tính toán ngày giao hàng dự kiến
            };

            // Trả về trang thông báo thanh toán thành công
            return View(viewModel);
        }



        public IActionResult EmptyCart()
        {
            return View();
        }

        private async Task<int> SaveOrder(PaymentModel model)
        {
            var order = new Order
            {
                UserId = 1, // TODO: Lấy từ đăng nhập thực tế
                TotalPrice = (decimal)model.CartItems.Sum(item => item.Product.Price * item.Quantity),
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in model.CartItems)
            {
                var orderItem = new OrdersItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product?.Price ?? 0
                };

                _context.OrdersItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            var userCart = _context.Carts.Where(c => c.UserId == 1);
            _context.Carts.RemoveRange(userCart);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        private async Task SavePayment(int orderId, string paymentMethod)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                UserId = 1, // TODO: lấy từ đăng nhập thực tế
                PaymentMethod = paymentMethod,
                PaymentStatus = "Processing",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
    }

    public class CartItemInputModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public decimal PriceValue { get; set; }
        public string ImageUrl { get; set; }
        public string Size { get; set; }
    }
}
