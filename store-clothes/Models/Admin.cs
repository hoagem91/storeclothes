using System;
using System.Collections.Generic;

namespace store_clothes.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
