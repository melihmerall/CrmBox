using System.Security.Claims;
using CrmBox.Core.Domain.Identity;

namespace CrmBox.WebUI.Models
{
    public class RoleWithSelectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
