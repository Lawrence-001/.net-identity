using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.ViewModels
{
    public class EditUserVM
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
