namespace Demo.Data;

using Microsoft.EntityFrameworkCore;
using Models.Data;

using static Common.Constants.ValidationConstants;

public sealed class LibraryDbContext(
    DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; init; }

    public DbSet<Author> Authors { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Author>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(AuthorNameMaxLength);

        modelBuilder
            .Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(BookTitleMaxLength);

        modelBuilder
            .Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
