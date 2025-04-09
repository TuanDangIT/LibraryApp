namespace Biblioteka.Models.DBEntities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Order Order { get; set; } = default!;
        public int OrderId { get; set; }
        public Book Book { get; set; } = default!;
        public int BookId { get; set; }
    }
}
