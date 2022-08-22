namespace CrmBox.WebUI.Models
{
    public class UpdateRoleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PolicyWithIsSelectedVM> Policies { get; set; }
    }
}
