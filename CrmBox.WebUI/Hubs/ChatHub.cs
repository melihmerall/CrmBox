
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
using CrmBox.Application.Interfaces;
using CrmBox.Application.Interfaces.Chat;

namespace CrmBox.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageService _chatMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<AgentHub> _agentHub;

        private static int Count = 0;
        public ChatHub(IChatRoomService chatRoomService, IHubContext<AgentHub> agentHub, IChatMessageService chatMessageService)
        {
            _chatRoomService = chatRoomService;
            _agentHub = agentHub;
            _chatMessageService = chatMessageService;

        }

        public override async Task OnConnectedAsync()
        {
            //
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }


            //
            var roomId = await _chatRoomService.CreateRoom(
                Context.ConnectionId);

            //
            await Groups.AddToGroupAsync(
                Context.ConnectionId, roomId.ToString());

            // first connection default messaage to customer.
            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                "Müşteri Destek Sistemi",
                DateTimeOffset.UtcNow,
                "Merhaba, Nasıl Yardımcı Olabilirim ?");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           
            await _chatRoomService.DeleteRoom(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        // sending message to customer base
        public async Task SendMessage(string name, string text)
        {

            var roomId = await
                _chatRoomService.GetRoomForConnectionId(
                    Context.ConnectionId);

            //
            var message = new ChatMessage()
            {
                SenderName = name,
                Text = text,
                SentDT = DateTimeOffset.UtcNow
            };
            await _chatRoomService.AddMessage(roomId, message);

            // save to database ChatRooms
            await _chatMessageService.AddAsync(message);
            // Broadcast to all clients
            await Clients.Group(roomId.ToString()).SendAsync(
                "ReceiveMessage",
                message.SenderName,
                message.SentDT,
                message.Text);
        }

        public async Task SetName(string visitorName, string visitorDepartment, string visitorMail)
        {
            var roomName = $"{visitorName}";
            var roomDepartment = $"{visitorDepartment}";
            var roomMail = $"{visitorMail}";

            var roomId = await _chatRoomService.GetRoomForConnectionId(
                Context.ConnectionId);
            ChatRoom chatRoom = new ChatRoom()
            {
                OwnerConnectionId = Context.ConnectionId,
                Mail = visitorMail,
                Name = visitorName,
                Department = visitorDepartment,
                CreatedTime = DateTime.Now
            };

            await _chatRoomService.SetRoomName(roomId, roomName, roomDepartment, roomMail);
            await _chatRoomService.AddAsync(chatRoom);

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




