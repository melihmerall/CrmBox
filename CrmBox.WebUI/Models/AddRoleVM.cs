using System.ComponentModel.DataAnnotations;

namespace CrmBox.WebUI.Models
{
    public class AddRoleVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen rol adını giriniz.")]
        public string Name { get; set; }

    }
}
