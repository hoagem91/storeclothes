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

        private int? GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return null;
            return int.TryParse(userIdString, out int id) ? id : (int?)null;
        }

        private async Task ClearCart(int userId)
        {
            try
            {
                Console.WriteLine($"[ClearCart] Starting to clear cart for userId = {userId}");

                var userCart = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                Console.WriteLine($"[ClearCart] Found {userCart.Count} items in cart");

                if (userCart.Any())
                {
                    _context.Carts.RemoveRange(userCart);
                    var result = await _context.SaveChangesAsync();
                    Console.WriteLine($"[ClearCart] Successfully removed {result} items from cart");
                }
                else
                {
                    Console.WriteLine("[ClearCart] Cart was already empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClearCart] Error clearing cart: {ex.Message}");
                throw;
            }
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
                FullName = User.Identity?.Name ?? "",
                Address = "",
                Phone = "",
                PaymentMethod = "COD"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaymentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Thêm thông báo lỗi validation
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                    return View(model);
                }

                if (model.CartItems == null || !model.CartItems.Any())
                {
                    ModelState.AddModelError("", "Giỏ hàng trống. Vui lòng thêm sản phẩm vào giỏ hàng.");
                    return View(model);
                }

                // Validate required fields
                if (string.IsNullOrEmpty(model.FullName))
                {
                    ModelState.AddModelError("FullName", "Vui lòng nhập họ và tên.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Address))
                {
                    ModelState.AddModelError("Address", "Vui lòng nhập địa chỉ giao hàng.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Phone))
                {
                    ModelState.AddModelError("Phone", "Vui lòng nhập số điện thoại.");
                    return View(model);
                }

                // Save order and payment
                try
                {
                    var orderId = await SaveOrder(model);
                    await SavePayment(orderId, model.PaymentMethod);
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    // Log error
                    Console.WriteLine($"Error saving order/payment: {ex.Message}");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xử lý đơn hàng. Vui lòng thử lại sau.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Unexpected error in payment process: {ex.Message}");
                return RedirectToAction("PaymentFailed", new { errorMessage = "Có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại sau." });
            }
        }

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
                    ViewBag.ErrorMessage = "Khong the tao URL thanh toan MoMo.";
                    return View("PaymentFailed");
                }
                return Redirect(response.PayUrl);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Loi khi tao thanh toan MoMo: {ex.Message}";
                return View("PaymentFailed");
            }
        }

        [HttpGet]
        [Route("Checkout/PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack()
        {
            string resultCode = Request.Query["resultCode"];
            string orderId = Request.Query["orderId"];
            string message = Request.Query["message"];

            if (resultCode == "0")
            {
                var payment = await _context.Payments
                    .Where(p => p.TransactionId == orderId)
                    .FirstOrDefaultAsync();
                if (payment != null)
                {
                    payment.PaymentStatus = "Completed";
                    await _context.SaveChangesAsync();

                    await ClearCart((int)payment.UserId);
                    return RedirectToAction("Success");
                }
                else
                {
                    ViewBag.ErrorMessage = "Khong tim thay giao dich.";
                    return RedirectToAction("PaymentFailed");
                }
            }
            else
            {
                ViewBag.ErrorMessage = $"Thanh toan that bai: {message}";
                return RedirectToAction("PaymentFailed");
            }
        }

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
                    await ClearCart((int)payment.UserId);
                }
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult StartCheckout([FromBody] List<CartItemInputModel> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
                return BadRequest("Gio hang trong.");

            TempData["CartData"] = JsonSerializer.Serialize(cartItems);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int userId, int productId, int quantity, decimal totalPrice, string paymentMethod)
        {
            if (userId <= 0 || productId <= 0 || quantity <= 0 || totalPrice <= 0 || string.IsNullOrEmpty(paymentMethod))
            {
                return BadRequest("Du lieu khong hop le.");
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

            await ClearCart(userId);

            return RedirectToAction("Success");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPaymentSuccess(PaymentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
                }

                if (model.CartItems == null || !model.CartItems.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                // Validate required fields
                if (string.IsNullOrEmpty(model.FullName))
                {
                    return Json(new { success = false, message = "Vui lòng nhập họ và tên" });
                }

                if (string.IsNullOrEmpty(model.Address))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ giao hàng" });
                }

                if (string.IsNullOrEmpty(model.Phone))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số điện thoại" });
                }

                // Save order and payment
                try
                {
                    var orderId = await SaveOrder(model);
                    await SavePayment(orderId, model.PaymentMethod);

                    var latestPayment = await _context.Payments
                        .Where(p => p.OrderId == orderId)
                        .FirstOrDefaultAsync();

                    if (latestPayment != null)
                    {
                        return Json(new { 
                            success = true, 
                            redirectUrl = Url.Action("Success", "Payments"),
                            paymentInfo = new {
                                transactionId = latestPayment.TransactionId,
                                paymentDate = latestPayment.PaymentDate,
                                estimatedDeliveryDate = DateTime.Now.AddDays(3)
                            }
                        });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không thể lưu thông tin thanh toán" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Lỗi khi xử lý đơn hàng: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi không mong muốn: {ex.Message}" });
            }
        }

        public IActionResult Success()
        {
            var viewModel = new PaymentSuccessViewModel
            {
                TransactionId = "TRX" + DateTime.Now.Ticks,
                PaymentDate = DateTime.Now,
                EstimatedDeliveryDate = DateTime.Now.AddDays(3)
            };

            return View(viewModel);
        }

        public IActionResult EmptyCart()
        {
            return View();
        }

        private async Task<int> SaveOrder(PaymentModel model)
        {
            var userId = GetCurrentUserId();
            if (userId == null) throw new Exception("Khong xac dinh duoc nguoi dung.");

            var order = new Order
            {
                UserId = userId.Value,
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
            await ClearCart(userId.Value);

            return order.Id;
        }

        private async Task SavePayment(int orderId, string paymentMethod)
        {
            var userId = GetCurrentUserId();
            if (userId == null) throw new Exception("Khong xac dinh duoc nguoi dung.");

            var payment = new Payment
            {
                OrderId = orderId,
                UserId = userId.Value,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Processing",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public IActionResult PaymentFailed()
        {
            return View();
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
