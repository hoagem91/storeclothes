using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Import thư viện Validation

namespace store_clothes.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống!")]
        [StringLength(50, ErrorMessage = "Tên tối đa 50 ký tự!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự!")]
        public string Password { get; set; } = null!;

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
