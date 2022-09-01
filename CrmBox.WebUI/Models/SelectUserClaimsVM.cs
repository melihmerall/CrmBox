namespace CrmBox.WebUI.Models
{
    public class SelectUserClaimsVM
    {
        public SelectUserClaimsVM()
        {
            Claims = new List<UserClaimVM>();
        }
        public int UserId { get; set; }
        public List<UserClaimVM> Claims { get; set; }
    }
}
