using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_clothes.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double OldPrice { get; set; }
        public int Discount { get; set; }
        public string Brand { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }


        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
