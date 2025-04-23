using System.Collections.Generic;

namespace store_clothes.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItem> OrderItems { get; set; }
    }
    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
