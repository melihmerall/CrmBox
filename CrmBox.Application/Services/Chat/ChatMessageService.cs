using CrmBox.Application.Interfaces.Customer;
using CrmBox.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmBox.Application.Interfaces.Chat;
using CrmBox.Core.Domain;

namespace CrmBox.Application.Services.Chat
{
    public class ChatMessageService : GenericService<ChatMessage, CrmBoxIdentityContext>, IChatMessageService
    {
        public ChatMessageService(CrmBoxIdentityContext context) : base(context)
        {
        }

    }
}
