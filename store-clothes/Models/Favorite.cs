using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace store_clothes.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}