namespace MVC.Identity.ViewModels.Administration
{
    public class UserRoleVm
    {
        public string UserId { get; set; } = string.Empty;
        //public string FName { get; set; }
        //public string LName { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
