using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteka.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        [DisplayName("Return status")]
        public bool IsReturned { get; set; }
        [DisplayName("Expiry Date")]
        public DateTime RentalDate { get; set; }
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        [DisplayName("Client First Name")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Client Last Name")]
        public string LastName { get; set; } = string.Empty;
    }
}
