using CrmBox.Core.Domain;

namespace CrmBox.WebUI.Models
{
    public class ChatVM
    {

        public int MaxRoomAllowed { get; set; }

        public string UserName { get; set; }
        public string? UserId { get; set; }

    }
}
