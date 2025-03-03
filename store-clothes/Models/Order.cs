using System;
using System.Collections.Generic;

namespace store_clothes.Models
{
    public partial class Order
    {
        public Order()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
