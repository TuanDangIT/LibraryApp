using Biblioteka.Models.DBEntities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteka.Models.DBEntities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime RealeaseDate { get; set; }
        public List<string> Keywords { get; set; } = new();
        public List<OrderItem> Items { get; set; } = new();
        public int Quantity { get; set; } 
    }
}
