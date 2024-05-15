using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.ViewModels.Product
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required(ErrorMessage = "Price cannot be empty")]
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
