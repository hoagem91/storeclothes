﻿using System;
using System.Collections.Generic;

namespace store_clothes.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public virtual Order? Order { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; } // Liên kết với CartItems (thay thế Order)
    }
}
