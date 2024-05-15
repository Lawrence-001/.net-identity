using Microsoft.AspNetCore.Identity;
using MVC.Identity.ViewModels.Administration;

namespace MVC.Identity.ViewModels
{
    public class CombinedVM
    {
        public IdentityRole IdentityRole { get; set; }
        public List<IdentityRole> IdentityRoles { get; set; }
        public EditRoleVM   EditRoleVM { get; set; }
    }
}
