using CrmBox.Application.Interfaces.Message;
using CrmBox.Core.Domain;
using CrmBox.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Application.Services.Message
{
    public class MessageService : GenericService<Core.Domain.Message, CrmBoxIdentityContext>, IMessageService
    {
        readonly CrmBoxIdentityContext _context;
        public MessageService(CrmBoxIdentityContext context) : base(context)
        {
            _context = context;
        }
    }
}
