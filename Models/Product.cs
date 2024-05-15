using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

    }
}
