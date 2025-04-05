using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace store_clothes.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            Favorites = new HashSet<Favorite>(); // Thêm khởi tạo cho Favorites
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}