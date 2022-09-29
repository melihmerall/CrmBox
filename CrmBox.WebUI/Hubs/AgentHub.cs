using CrmBox.Application.Interfaces.Chat;
using CrmBox.Application.Services.Chat;
using CrmBox.Core.Domain;
using CrmBox.Persistance.Context;
using Microsoft.AspNetCore.SignalR;

namespace CrmBox.WebUI.Hubs
{
    public class AgentHub : Hub
    {
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<ChatHub> _chatHub;


        public AgentHub(
            IChatRoomService chatRoomService,
            IHubContext<ChatHub> chatHub, IChatMessageService chatMessageService)
        {
            _chatRoomService = chatRoomService;
            _chatHub = chatHub;

            _chatMessageService = chatMessageService;

        }

        public override async Task OnConnectedAsync()
        {

            // Get all active rooms
            await Clients.Caller.SendAsync(
                "ActiveRooms",
                await _chatRoomService.GetAllRooms());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            

            await base.OnDisconnectedAsync(exception);
        }

        // operatör tarafı mesaj gönderme
        public async Task SendAgentMessage(Guid roomId, string text)
        {
            ChatMessage message = new ChatMessage
            {
                SenderName = Context.User.Identity.Name,
                Text = text,
                SentDT = DateTimeOffset.UtcNow

            };

            await _chatRoomService.AddMessage(roomId, message);

            // Write to database.
            await _chatMessageService.AddAsync(message);

            await _chatHub.Clients
                .Group(roomId.ToString())
                .SendAsync("ReceiveMessage",
                    message.SenderName,
                    message.SentDT,
                    message.Text);
        }
        public async Task LoadHistory(Guid roomId)
        {
            var history = await _chatRoomService
                .GetMessageHistory(roomId);

            await Clients.Caller.SendAsync(
                "ReceiveMessages", history);
        }

    }
}
