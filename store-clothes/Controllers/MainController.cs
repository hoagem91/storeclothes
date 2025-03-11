using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using store_clothes.Models;

namespace store_clothes.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product { Name = "Shiny Dress", Brand = "Al Karam", Price = 95.50m, OldPrice = 120.00, Discount = 20, ImageUrl = "/images/shiny-dress.jpg", CategoryId = 1 },
                new Product { Name = "Long Dress", Brand = "Al Karam", Price = 85.00m, OldPrice = 100.00, Discount = 15, ImageUrl = "/images/long-dress.jpg", CategoryId = 1 },
                new Product { Name = "Full Sweater", Brand = "Al Karam", Price = 75.00m, OldPrice = 90.00, Discount = 10, ImageUrl = "/images/full-sweater.jpg", CategoryId = 1 },
                new Product { Name = "White Dress", Brand = "Al Karam", Price = 95.50m, OldPrice = 110.00, Discount = 10, ImageUrl = "/images/white-dress.jpg", CategoryId = 1 },
                new Product { Name = "Colorful Dress", Brand = "Al Karam", Price = 90.00m, OldPrice = 105.00, Discount = 10, ImageUrl = "/images/colorful-dress.jpg", CategoryId = 1 },
                new Product { Name = "White Shirt", Brand = "Al Karam", Price = 50.00m, OldPrice = 60.00, Discount = 15, ImageUrl = "/images/white-shirt.jpg", CategoryId = 1 },

                new Product { Name = "Office Shirt", Brand = "Al Karam", Price = 65.00m, OldPrice = 80.00, Discount = 10, ImageUrl = "/images/Office-Shirt.jpeg", CategoryId = 2 },
                new Product { Name = "Denim Jacket", Brand = "Al Karam", Price = 110.00m, OldPrice = 130.00, Discount = 15, ImageUrl = "/images/denim-jacket.jpg", CategoryId = 2 },

                new Product { Name = "Charis Bag", Brand = "Al Karam", Price = 120.00m, OldPrice = 140.00, Discount = 10, ImageUrl = "/images/Bag.jpg", CategoryId = 3 },
                new Product { Name = "Wool Scraft", Brand = "Al Karam", Price = 30.00m, OldPrice = 40.00, Discount = 10, ImageUrl = "/images/scraft.jpg", CategoryId = 3 },
                new Product { Name = "Sun Hat", Brand = "Al Karam", Price = 25.00m, OldPrice = 35.00, Discount = 10, ImageUrl = "/images/mu.jpg", CategoryId = 3 },

                new Product { Name = "Leather Belt", Brand = "Al Karam", Price = 40.00m, OldPrice = 50.00, Discount = 10, ImageUrl = "/images/belt.webp", CategoryId = 4 },
                new Product { Name = "Gold Jewelry", Brand = "Al Karam", Price = 200.00m, OldPrice = 250.00, Discount = 15, ImageUrl = "/images/nhan.jpg", CategoryId = 4 },
                new Product { Name = "Sunglasses", Brand = "Al Karam", Price = 80.00m, OldPrice = 100.00, Discount = 15, ImageUrl = "/images/kinh.webp", CategoryId = 4 }
            };

            return View(products);
        }
    }
}
