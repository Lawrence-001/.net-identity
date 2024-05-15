using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.ViewModels.Auth
{
    public class RegistrationVM
    {
        [Required(ErrorMessage = "First Name cannot be empty")]
        [Display(Name = "First Name")]
        public string FName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name cannot be empty")]
        [Display(Name = "Last Name")]
        public string LName { get; set; } = string.Empty;

        [Required (ErrorMessage ="Email cannot be empty")]
        [EmailAddress]
        [Remote(action: "IsEmailTaken", controller: "Auth")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password cannot be empty")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
    }
}
