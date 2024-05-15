using System.ComponentModel.DataAnnotations;

namespace MVC.Identity.ViewModels.Administration
{
    public class EditRoleVM
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; } = string.Empty;
        public List<string>? Users { get; set; }
    }
}
