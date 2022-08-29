using CrmBox.Core.Domain.Base;
using CrmBox.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Core.Domain
{
    public class Message : BaseEntity
    {
        public string MessageText { get; set; }
        public DateTime MessageTime { get; set; }
        public virtual AppUser FromUser { get; set; }
    }
}
