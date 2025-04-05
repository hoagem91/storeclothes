namespace store_clothes.Models
{
    public class PaymentModel
    {
        public List<CartItem> CartItems { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }
    }
}
