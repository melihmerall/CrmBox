
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
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        private readonly CrmBoxIdentityContext _context;
        private readonly IMapper _mapper;

        public ChatHub(CrmBoxIdentityContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        private readonly CrmBoxIdentityContext _db;
        public ChatHub(CrmBoxIdentityContext db)
        {
            _db = db;
        }



        public async Task SendPrivate(string receiverName, string message)
        {
            if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
            {
                // Who is the sender;
                var sender = _Connections.Where(u => u.Username == IdentityName).First();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Build the message
                    var messageViewModel = new MessageVM()
                    {
                        Content = Regex.Replace(message, @"<.*?>", string.Empty),
                        From = sender.FirstName + sender.LastName,
                        Avatar = sender.Avatar,
                        Room = "",
                        Timestamp = DateTime.Now
                    };

                    // Send the message
                    await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }

        public async Task Join(string roomName)
        {
            try
            {
                var user = _Connections.Where(u => u.Username == IdentityName).FirstOrDefault();
                if (user != null && user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    // Join to new chat room
                    await Leave(user.CurrentRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public IEnumerable<UserProfileVM> GetUsers(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
                var userViewModel = _mapper.Map<AppUser, UserProfileVM>(user);
                userViewModel.Device = GetDevice();
                userViewModel.CurrentRoom = "";

                if (!_Connections.Any(u => u.Username == IdentityName))
                {
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
                }

                Clients.Caller.SendAsync("getProfileInfo", user.FirstName, user.Avatar);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _Connections.Where(u => u.Username == IdentityName).First();
                _Connections.Remove(user);

                // Tell other users to remove you from their list
                Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                // Remove mapping
                _ConnectionsMap.Remove(user.Username);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        private string GetDevice()
        {
            var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
            if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }






































        //public override Task OnConnectedAsync()
        //{
        //    var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (!String.IsNullOrEmpty(UserId))
        //    {
        //        var userName = _db.Users.FirstOrDefault(u => u.Id.ToString() == UserId).UserName;
        //        Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", UserId, userName);
        //        HubConnections.AddUserConnection(UserId, Context.ConnectionId);
        //    }
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (HubConnections.HasUserConnection(UserId, Context.ConnectionId))
        //    {
        //        var UserConnections = HubConnections.Users[UserId];
        //        UserConnections.Remove(Context.ConnectionId);

        //        HubConnections.Users.Remove(UserId);
        //        if (UserConnections.Any())
        //            HubConnections.Users.Add(UserId, UserConnections);
        //    }

        //    if (!String.IsNullOrEmpty(UserId))
        //    {
        //        var userName = _db.Users.FirstOrDefault(u => u.Id.ToString() == UserId).UserName;
        //        Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", UserId, userName);
        //        HubConnections.AddUserConnection(UserId, Context.ConnectionId);
        //    }
        //    return base.OnDisconnectedAsync(exception);
        //}


        public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id.ToString() == senderId).UserName;

            var users = new string[] { senderId, receiverId };

            await Clients.Users(users).SendAsync("ReceivePrivateMessage", senderId, senderName, receiverId, message, Guid.NewGuid(), receiverName);
        }

        public async Task SendOpenPrivateChat(string receiverId)
        {
            var username = Context.User.FindFirstValue(ClaimTypes.Name);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await Clients.User(receiverId).SendAsync("ReceiveOpenPrivateChat", userId, username);
        }

        public async Task SendDeletePrivateChat(string chartId)
        {
            await Clients.All.SendAsync("ReceiveDeletePrivateChat", chartId);
        }

        //public async Task SendMessageToAll(string user, string message)
        //{
        //    await Clients.All.SendAsync("MessageReceived", user, message);
        //}
        //[Authorize]
        //public async Task SendMessageToReceiver(string sender, string receiver, string message)
        //{
        //    var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        await Clients.User(userId).SendAsync("MessageReceived", sender, message);
        //    }

        //}

    }

}



