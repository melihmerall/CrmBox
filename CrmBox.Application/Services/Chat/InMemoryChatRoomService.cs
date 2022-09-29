using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmBox.Application.Interfaces.Chat;
using CrmBox.Core.Domain;
using CrmBox.Persistance.Context;

namespace CrmBox.Application.Services.Chat
{
    public class InMemoryChatRoomService : GenericService<ChatRoom, CrmBoxIdentityContext> ,IChatRoomService
    {
        readonly CrmBoxIdentityContext _context;

        public InMemoryChatRoomService(CrmBoxIdentityContext context ) : base(context)
        {
            _context = context;
  
        }


        private readonly Dictionary<Guid, ChatRoom> _roomInfo
            = new Dictionary<Guid, ChatRoom>();

        private readonly Dictionary<Guid,List<ChatMessage>>
            _messageHistory= new Dictionary<Guid, List<ChatMessage>>();
        public Task<Guid> CreateRoom(string connectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoom
            {
                OwnerConnectionId = connectionId

            };
            return Task.FromResult(id);

        }

        public Task<Guid> DeleteRoom(string connectionId)
        {
            // if customer close the client, i take that connId and remove to dictionary.
            // in this way, customer room is remove on my operator screen.
            var foundRoom = _roomInfo.FirstOrDefault(
                x => x.Value.OwnerConnectionId == connectionId);
            _roomInfo.Remove(foundRoom.Key);//remove from dictionary.

            var chatRoom = _context.Set<ChatRoom>().FirstOrDefault(c=>c.OwnerConnectionId==connectionId);
            if (chatRoom != null)
            {// if chatRoom is not null, remove to database, because user disconnect.
                _context.Set<ChatRoom>().Remove(chatRoom);
                _context.SaveChanges();
                return (Task<Guid>)Task.CompletedTask;
            }

            return (Task<Guid>)Task.CompletedTask;
        }


        public Task<Guid> GetRoomForConnectionId(string connectionId)
        {
            var foundRoom = _roomInfo.FirstOrDefault(
                x => x.Value.OwnerConnectionId == connectionId);

            if (foundRoom.Key == Guid.Empty)
            {
                throw new ArgumentException("Invalid Connection Id");

            }

            return Task.FromResult(foundRoom.Key);
        }


        public Task SetRoomName(Guid roomId, string name,string department,string mail)
        {
            if (!_roomInfo.ContainsKey(roomId))
                throw new ArgumentException("Invalid room Id");

            _roomInfo[roomId].Name = name;
            _roomInfo[roomId].Department = department;
            _roomInfo[roomId].Mail = mail;
            return Task.CompletedTask;
        }

        public Task AddMessage(Guid roomId, ChatMessage message)
        {
            if (!_messageHistory.ContainsKey(roomId))
            {
                _messageHistory[roomId] = new List<ChatMessage>();
            }
            _messageHistory[roomId].Add(message);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid roomId)
        {
            _messageHistory.TryGetValue(roomId, out var messages);
            messages = messages ?? new List<ChatMessage>();

            var sortedMessages = messages
                .OrderBy(x => x.SentDT)
                .AsEnumerable();

            return Task.FromResult(sortedMessages);
        }

        public Task<IReadOnlyDictionary<Guid, ChatRoom>> GetAllRooms()
        {
            return Task.FromResult(
                _roomInfo as IReadOnlyDictionary<Guid, ChatRoom>);
        }
  

    }
}
