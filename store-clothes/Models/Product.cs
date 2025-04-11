using System;
using System.Collections.Generic;

namespace store_clothes.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            Carts = new HashSet<Cart>();
            Favorites = new HashSet<Favorite>();
            OrdersItems = new HashSet<OrdersItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Size { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<OrdersItem> OrdersItems { get; set; }
    }
}
