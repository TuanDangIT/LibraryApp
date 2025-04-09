using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteka.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
