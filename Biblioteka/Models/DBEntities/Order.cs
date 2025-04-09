using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Biblioteka.Models.DBEntities;
namespace Biblioteka.Models.DBEntities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public bool IsReturned { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime RentalExpireDate { get; set; }
        public Client Client { get; set; } = default!;
        public int ClientId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
