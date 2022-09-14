
using AutoMapper;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.Persistance.Context;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using CrmBox.Application.Interfaces.Chat;

namespace CrmBox.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<AgentHub> _agentHub;

        public ChatHub(IChatRoomService chatRoomService, IHubContext<AgentHub> agentHub)
        {
            _chatRoomService = chatRoomService;
            _agentHub = agentHub;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }
            var roomId = await _chatRoomService.CreateRoom(
                Context.ConnectionId);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,roomId.ToString());

            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                "Müşteri Destek Sistemi",
                DateTimeOffset.UtcNow,
                "Merhaba, Nasıl Yardımcı Olabilirim ?");

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string name, string text)
        {

            var roomId = await
                _chatRoomService.GetRoomForConnectionId(
                    Context.ConnectionId);


            var message = new ChatMessage()
            {
                SenderName = name,
                Text = text,
                SentDT = DateTimeOffset.UtcNow
            };
            await _chatRoomService.AddMessage(roomId, message);



            // Broadcast to all clients
            await Clients.Group(roomId.ToString()).SendAsync(
                "ReceiveMessage",
                message.SenderName,
                message.SentDT,
                message.Text);
        }

        public async Task SetName(string visitorName)
        {
            var roomName = $"Müşteri Adı: {visitorName}";
            

            var roomId = await _chatRoomService.GetRoomForConnectionId(
                Context.ConnectionId);

            await _chatRoomService.SetRoomName(roomId, roomName);

            await _agentHub.Clients.All
                .SendAsync(
                    "ActiveRooms",
                    await _chatRoomService.GetAllRooms());
        }

        public async Task JoinRoom(Guid roomId)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentException("invalid id");
            }

            await Groups.AddToGroupAsync(
                Context.ConnectionId, roomId.ToString());
        }
        public async Task LeaveRoom(Guid roomId)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentException("invalid id");
            }

            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId, roomId.ToString());
        }

    }

}



