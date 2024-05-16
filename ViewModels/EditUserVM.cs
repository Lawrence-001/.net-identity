using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.ViewModels
{
    public class EditUserVM
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required (ErrorMessage ="Fisrt Name cannot be empty")]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;
        [Required(ErrorMessage ="Last Name cannot be empty")]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;
        [Required (ErrorMessage ="Email cannot be empty")]
        [EmailAddress(ErrorMessage ="Email must have @ symbol eg example@mail.com")]
        public string Email { get; set; } = string.Empty;
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
