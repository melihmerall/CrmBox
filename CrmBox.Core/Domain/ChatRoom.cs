using CrmBox.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmBox.Core.Domain.Base;

namespace CrmBox.Core.Domain
{
    public class ChatRoom : BaseEntity
    {
        public string OwnerConnectionId { get; set; }
        public string Name { get; set; }
    }
}
