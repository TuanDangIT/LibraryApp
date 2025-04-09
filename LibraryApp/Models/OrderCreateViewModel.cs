using System.ComponentModel;

namespace Biblioteka.Models
{
    public class OrderCreateViewModel
    {
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        public List<OrderBookCreateViewModel>? Books { get; set; } 
        public List<OrderClientCreateViewModel>? Clients { get; set; } 
        public int ClientId { get; set; }
        public List<int> BookIds { get; set; } = new();
    }
}
