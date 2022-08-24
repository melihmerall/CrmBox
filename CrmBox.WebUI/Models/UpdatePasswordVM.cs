using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CrmBox.WebUI.Models
{
    public class UpdatePasswordVM
    {
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Lütfen şifreyi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
