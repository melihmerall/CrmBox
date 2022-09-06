using System.ComponentModel.DataAnnotations;

namespace CrmBox.WebUI.Models
{
    public class MessageVM
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string From { get; set; }
        [Required]
        public string Room { get; set; }
        public string Avatar { get; set; }
    }
}
