using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace store_clothes.Models
{
    public partial class User
    {
        public User()
        {
            Cartitems = new HashSet<CartItem>();
            Carts = new HashSet<Cart>();
            Favorites = new HashSet<Favorite>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<CartItem> Cartitems { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
