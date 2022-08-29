using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CrmBox.Core.Domain.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        // Bire çok ilişki   AppUser || Mesasge
        public virtual ICollection<Message> Messages { get; set; }
  



    }
}
