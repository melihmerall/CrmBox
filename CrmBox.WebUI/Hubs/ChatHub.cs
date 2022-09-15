
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

namespace CrmBox.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        public readonly static List<UserProfileVM> _Connections = new List<UserProfileVM>();

        private readonly CrmBoxIdentityContext _db;
        public ChatHub(CrmBoxIdentityContext db)
        {
            _db = db;
        }

        public override Task OnConnectedAsync()
        {
            //var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (!String.IsNullOrEmpty(UserId))
            //{
            //    var userName = _db.Users.FirstOrDefault(u => u.Id.ToString() == UserId).UserName;
            //    Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", UserId, userName);
            //    HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            //}
            return base.OnConnectedAsync();
        }

        public async  Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("receiveMessages", message);
        }

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //if (HubConnections.HasUserConnection(UserId, Context.ConnectionId))
            //{
            //    var UserConnections = HubConnections.Users[UserId];
            //    UserConnections.Remove(Context.ConnectionId);

            //    HubConnections.Users.Remove(UserId);
            //    if (UserConnections.Any())
            //        HubConnections.Users.Add(UserId, UserConnections);
            //}

            //if (!String.IsNullOrEmpty(UserId))
            //{
            //    var userName = _db.Users.FirstOrDefault(u => u.Id.ToString() == UserId).UserName;
            //    Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", UserId, userName);
            //    HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            //}
            //return base.OnDisconnectedAsync(exception);
        }

        //public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        //{
        //    var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var senderName = _db.Users.FirstOrDefault(u => u.Id.ToString() == senderId).UserName;
        //    _db.Add(message);
        //    var users = new string[] { senderId, receiverId };

        //    await Clients.Users(users).SendAsync("ReceivePrivateMessage", senderId, senderName, receiverId, message, Guid.NewGuid(), receiverName);
        //}

        //public async Task SendOpenPrivateChat(string receiverId)
        //{
        //    var username = Context.User.FindFirstValue(ClaimTypes.Name);
        //    var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    await Clients.User(receiverId).SendAsync("ReceiveOpenPrivateChat", userId, username);
        //}

        //public async Task SendDeletePrivateChat(string chartId)
        //{
        //    await Clients.All.SendAsync("ReceiveDeletePrivateChat", chartId);
        //}

        //public async Task SendMessageToAll(string user, string message)
        //{
        //    await Clients.All.SendAsync("MessageReceived", user, message);
        //}
        
        //public async Task SendMessageToReceiver(string sender, string receiver, string message)
        //{
        //    var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        await Clients.User(userId).SendAsync("MessageReceived", sender, message);
        //    }

        //}

    }
    




