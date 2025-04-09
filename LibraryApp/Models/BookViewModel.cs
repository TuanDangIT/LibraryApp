using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteka.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        [DisplayName("Author")]
        public string Author { get; set; } = string.Empty;
        [DisplayName("Title")]
        public string Title { get; set; } = string.Empty;
        [DisplayName("Realease date")]
        public DateTime RealeaseDate { get; set; }
        [DisplayName("Keywords")]
        public string Keywords { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
