using System.ComponentModel;

namespace Biblioteka.Models
{
    public class ClientShortenedViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
