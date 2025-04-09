using Biblioteka.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
namespace Biblioteka.DAL
{
    public class BibliotekaDbContext : DbContext
    {
        public BibliotekaDbContext(DbContextOptions<BibliotekaDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);
            //modelBuilder.Entity<Book>()
            //    .Property(b => b.Category)
            //    .HasConversion<string>();
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Items)
                .WithOne(i => i.Book)
                .HasForeignKey(i => i.BookId);
            modelBuilder.Entity<Book>()
                .Property(b => b.Keywords)
                .HasConversion(tags => string.Join(',', tags), tags => tags.Split(',', StringSplitOptions.None).ToList());
            modelBuilder.Entity<Order>()
                .HasMany(b => b.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId); ;
            modelBuilder.Entity<Client>()
                .HasMany(b => b.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(i => i.ClientId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
