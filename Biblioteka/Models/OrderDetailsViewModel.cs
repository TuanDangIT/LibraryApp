using System.ComponentModel;

namespace Biblioteka.Models
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Return status")]
        public bool IsReturned { get; set; }
        [DisplayName("Expiry Date")]
        public DateTime RentalDate { get; set; }
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        public ClientShortenedViewModel ClientShortened { get; set; } = default!;
        public List<BookShortenedViewModel> Books { get; set; } = new();
    }
}
