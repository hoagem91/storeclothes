using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_clothes.Models
{
    [Table("CartItem")] // Gán với bảng CartItem trong MySQL
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Column("user_id")]
        public virtual User? User { get; set; }

        public int? UserId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal Total => (Product?.Price ?? 0) * Quantity;

    }
}