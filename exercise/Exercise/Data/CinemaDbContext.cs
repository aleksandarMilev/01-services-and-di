namespace Exercise.Data;

using Microsoft.EntityFrameworkCore;
using Models.Data;

using static Common.Constants.ValidationConstants;

public sealed class CinemaDbContext(
    DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<MovieDataModel> Movies { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<MovieDataModel>()
            .Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(MovieTitleMaxLength);

        modelBuilder
            .Entity<MovieDataModel>()
            .Property(m => m.Genre)
            .IsRequired()
            .HasMaxLength(MovieGenreMaxLength);
    }
}