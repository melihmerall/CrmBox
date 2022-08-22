namespace CrmBox.WebUI.Models
{
    public class AddUserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<RoleWithSelectVM> Roles { get; set; }
    }
}
