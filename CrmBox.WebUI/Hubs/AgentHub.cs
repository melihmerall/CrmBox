using CrmBox.Application.Interfaces.Chat;
using CrmBox.Core.Domain;
using Microsoft.AspNetCore.SignalR;

namespace CrmBox.WebUI.Hubs
{
    public class AgentHub :Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<ChatHub> _chatHub;

        public AgentHub(
            IChatRoomService chatRoomService,
            IHubContext<ChatHub> chatHub)
        {
            _chatRoomService = chatRoomService;
            _chatHub = chatHub;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync(
                "ActiveRooms",
                await _chatRoomService.GetAllRooms());

            await base.OnConnectedAsync();
        }

        public async Task SendAgentMessage(Guid roomId, string text)
        {
            var message = new ChatMessage
            {
                SenderName = Context.User.Identity.Name,
                Text = text,
                SentDT  = DateTimeOffset.UtcNow
            };

            await _chatRoomService.AddMessage(roomId, message);

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
