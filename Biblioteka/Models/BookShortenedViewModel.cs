using System.ComponentModel;

namespace Biblioteka.Models
{
    public class BookShortenedViewModel
    {
        public string Category { get; set; } = string.Empty;
        [DisplayName("Author")]
        public string Author { get; set; } = string.Empty;
        [DisplayName("Title")]
        public string Title { get; set; } = string.Empty;
        [DisplayName("Realease date")]
        public DateTime RealeaseDate { get; set; }
        [DisplayName("Keywords")]
        public string Keywords { get; set; } = string.Empty;
    }
}
