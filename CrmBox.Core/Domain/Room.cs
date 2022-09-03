using CrmBox.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Core.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUser Admin { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
