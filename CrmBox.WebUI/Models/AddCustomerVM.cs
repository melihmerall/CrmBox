namespace CrmBox.WebUI.Models
{
    public class AddCustomerVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedTime { get; set; }
        public string JobTitle { get; set; }
    }
}
