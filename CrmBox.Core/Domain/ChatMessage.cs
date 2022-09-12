using CrmBox.Core.Domain.Base;
using CrmBox.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Core.Domain
{
    public class ChatMessage : BaseEntity
    {
  
        public string SenderName { get; set; }

        public string Text { get; set; }

        public DateTimeOffset SentDT { get; set; }
    }
}
