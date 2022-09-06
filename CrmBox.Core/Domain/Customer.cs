using CrmBox.Core.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace CrmBox.Core.Domain;

public class Customer : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required(ErrorMessage = "Mobile Number is required.")]
    [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string CompanyName { get; set; }
    public string JobTitle { get; set; }

}