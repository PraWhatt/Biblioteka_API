using Biblioteka.Models;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Copy> Copies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(e => {
                e.Property(a => a.FirstName).IsRequired();
                e.Property(a => a.LastName).IsRequired();
            });
            modelBuilder.Entity<Book>(e => {
                e.Property(b => b.Title).IsRequired();
                e.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Copy>(e => {
                e.HasOne(c => c.Book)
                  .WithMany(b => b.Copies)
                  .HasForeignKey(c => c.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
