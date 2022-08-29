using CrmBox.Application.Services.Message;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.Persistance.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace CrmBox.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MessageService _messageService;
        private readonly CrmBoxIdentityContext _context;
        public ChatHub(UserManager<AppUser> userManager, CrmBoxIdentityContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task BroadcastFromClient(string message)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(Context.User);
                Message m = new Message()
                {
                    MessageText = message,
                    MessageTime = DateTime.Now,
                    FromUser = currentUser
                };
                _context.Add(m);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync(
                    "Broadcast",
                    new
                    {
                        messageText = m.MessageText,
                        fromUser = currentUser.Email,
                        messageTime = m.MessageTime.ToString(
                            "hh:mm tt MMM dd", CultureInfo.InvariantCulture)
                    });
            }
            catch (Exception ex)
            {

                await Clients.Caller.SendAsync("HubError", new { error = ex.Message });
            }
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync(
                "UserConnected",
                new
                {
                    connectionId = Context.ConnectionId,
                    connectionDt = DateTime.Now,
                    messageDt = DateTime.Now.ToString(
                            "hh:mm tt MMM dd", CultureInfo.InvariantCulture
                            )
                });

        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected",
                $"User disconnected, ConnectionId: {Context.ConnectionId}");
            
        }

    }
}
