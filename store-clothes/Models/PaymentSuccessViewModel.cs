using System;
using System.Collections.Generic;

namespace store_clothes.Models
{
    public class PaymentSuccessViewModel
    {
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        public List<CartItem> CartItems { get; set; } 


    }
    public class CartItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
