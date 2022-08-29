using CrmBox.Core.Domain;
using Microsoft.AspNetCore.SignalR;

namespace CrmBox.WebUI.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(Message message) =>
            await Clients.All.SendAsync("receiveMessage", message);
    }
}
