using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_clothes.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            var cartItems = await GetCartItems();
            if (!cartItems.Any())
            {
                return RedirectToAction("EmptyCart");
            }

            var model = new PaymentModel
            {
                CartItems = cartItems,
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
            if (!ModelState.IsValid)
            {
                model.CartItems = await GetCartItems();
                return View(model);
            }

            var orderId = await SaveOrder(model);
            await SavePayment(orderId, model.PaymentMethod);

            return RedirectToAction("Success");
        }

        private async Task<List<CartItem>> GetCartItems()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)  // ✅ JOIN với bảng products
                .ToListAsync();

            // ✅ Chuyển đổi Cart -> CartItem
            return cartItems.Select(c => new CartItem
            {
                Id = c.Id,
                UserId = c.UserId,
                ProductId = c.ProductId,
                Quantity = c.Quantity,
                Product = c.Product,
            }).ToList();
        }


        // ✅ Lưu đơn hàng vào bảng `orders` và `orders_items`
        private async Task<int> SaveOrder(PaymentModel model)
        {
            var order = new Order
            {
                UserId = 1,
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
                    Price = item.Product?.Price ?? 0 // ✅ Lấy giá từ `Product.Price`
                };

                _context.OrdersItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            // ✅ Xóa giỏ hàng của UserId sau khi tạo đơn hàng
            var userCart = _context.Carts.Where(c => c.UserId == 1);
            _context.Carts.RemoveRange(userCart);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        // ✅ Lưu thông tin thanh toán vào bảng `Payment`
        private async Task SavePayment(int orderId, string paymentMethod)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                UserId = 1,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Processing",
                TransactionId = null
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                OrderId = order.Id,
                UserId = userId,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Completed",
                TransactionId = Guid.NewGuid().ToString()
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult EmptyCart()
        {
            return View();
        }
    }
}
