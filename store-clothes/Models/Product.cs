using System.ComponentModel.DataAnnotations;

namespace store_clothes.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc!")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc!")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục cho sản phẩm!")]
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Size { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}