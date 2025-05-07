using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using store_clothes.Services.Momo;
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
        private readonly IMomoService _momoService;

        public PaymentsController(storeclothesContext context, IMomoService momoService)
        {
            _context = context;
            _momoService = momoService;
        }

        // GET: /Payments/
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
                FullName = User.Identity?.Name ?? "",
                Address = "",
                Phone = "",
                PaymentMethod = "COD"
            };

            return View(model);
        }
        // POST: /Payments/
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

        // POST: /Payments/CreatePaymentUrl
        [HttpPost]
        [Route("CreatePaymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl([FromForm] string FullName, [FromForm] long Amount, [FromForm] string OrderInfo)
        {
            var model = new OrderInfo
            {
                FullName = FullName,
                Amount = Amount,
                OrderInfomation = OrderInfo
            };

            try
            {
                var response = await _momoService.CreatePaymentMomo(model);
                if (string.IsNullOrEmpty(response.PayUrl))
                {
                    ViewBag.ErrorMessage = "Không thể tạo URL thanh toán MoMo.";
                    return View("PaymentFailed");
                }
                return Redirect(response.PayUrl);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi khi tạo thanh toán MoMo: {ex.Message}";
                return View("PaymentFailed");
            }
        }

        // GET: /Checkout/PaymentCallBack
        [HttpGet]
        [Route("Checkout/PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack()
        {
            string resultCode = Request.Query["resultCode"];
            string orderId = Request.Query["orderId"];
            string message = Request.Query["message"];

            if (resultCode == "0") // Success
            {
                var payment = await _context.Payments
                    .Where(p => p.TransactionId == orderId)
                    .FirstOrDefaultAsync();
                if (payment != null)
                {
                    payment.PaymentStatus = "Completed";
                    await _context.SaveChangesAsync();

                    // Xóa giỏ hàng của người dùng
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    int userId = int.TryParse(userIdString, out int id) ? id : 1; // Fallback to 1 if not authenticated
                    await ClearCart(userId);

                    return RedirectToAction("Success","Checkout");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không tìm thấy giao dịch.";
                    return RedirectToAction("PaymentFailed", "Checkout");
                }
            }
            else
            {
                ViewBag.ErrorMessage = $"Thanh toán thất bại: {message}";
                return RedirectToAction("PaymentFailed", "Checkout");
            }
        }
        // POST: /Checkout/MomoNotify
        [HttpPost]
        [Route("Checkout/MomoNotify")]
        [HttpPost]
        [Route("Checkout/MomoNotify")]
        public async Task<IActionResult> MomoNotify()
        {
            string resultCode = Request.Form["resultCode"];
            string orderId = Request.Form["orderId"];

            if (resultCode == "0")
            {
                var payment = await _context.Payments
                    .Where(p => p.TransactionId == orderId)
                    .FirstOrDefaultAsync();
                if (payment != null)
                {
                    payment.PaymentStatus = "Completed";
                    await _context.SaveChangesAsync();

                    // Xóa giỏ hàng của người dùng
                    int userId = (int)payment.UserId; // payment.UserId đã là kiểu int
                    if (userId <= 0)
                    {
                        Console.WriteLine($"Invalid userId: {userId}");
                    }
                    else
                    {
                        await ClearCart(userId);
                    }
                }
            }

            return Ok();
        }

        // POST: /Payments/StartCheckout
        [HttpPost]
        public IActionResult StartCheckout([FromBody] List<CartItemInputModel> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                return BadRequest("Giỏ hàng trống.");

            TempData["CartData"] = JsonSerializer.Serialize(cartItems);
            return Json(new { success = true });
        }

        // POST: /Payments/ProcessPayment
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
                OrderId = order.Id,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Completed",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi thanh toán thành công
            await ClearCart(userId);

            return RedirectToAction("Success");
        }

        // GET: /Payments/Success
        public async Task<IActionResult> Success()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Index", "Home");
            }

            var latestPayment = await _context.Payments
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefaultAsync();

            if (latestPayment == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new PaymentSuccessViewModel
            {
                TransactionId = latestPayment.TransactionId ?? "N/A",
                PaymentDate = latestPayment.PaymentDate,
                EstimatedDeliveryDate = DateTime.Now.AddDays(3)
            };

            return View(viewModel);
        }

        // GET: /Payments/EmptyCart
        public IActionResult EmptyCart()
        {
            return View();
        }

        // Phương thức lưu đơn hàng
        private async Task<int> SaveOrder(PaymentModel model)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.TryParse(userIdString, out int id) ? id : 1; // Fallback to 1 if not authenticated

            var order = new Order
            {
                UserId = userId,
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

            // Xóa giỏ hàng
            await ClearCart(userId);

            return order.Id;
        }

        // Phương thức lưu thanh toán
        private async Task SavePayment(int orderId, string paymentMethod)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.TryParse(userIdString, out int id) ? id : 1;

            var payment = new Payment
            {
                OrderId = orderId,
                UserId = userId,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Processing",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        // Phương thức xóa giỏ hàng (tối ưu hóa)
        private async Task ClearCart(int userId)
        {
            var userCart = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();
            if (userCart.Any())
            {
                _context.Carts.RemoveRange(userCart);
                await _context.SaveChangesAsync();
            }
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