using daper_54.Entities;
using Microsoft.EntityFrameworkCore;

namespace daper_54.Data;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<UserMovieView> UserMovies => Set<UserMovieView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Username).IsRequired().HasMaxLength(50);
            entity.Property(user => user.Email).IsRequired().HasMaxLength(254);
            entity.Property(user => user.Password).IsRequired();
            entity.HasIndex(user => user.Username).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies");
            entity.HasKey(movie => movie.Id);
            entity.Property(movie => movie.Title).IsRequired().HasMaxLength(50);
            entity.Property(movie => movie.ReleaseYear).IsRequired();
            entity.Property(movie => movie.Description).IsRequired(false);
            entity.Property(movie => movie.AddedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            entity.HasOne(movie => movie.User)
                .WithMany(user => user.AddedMovies)
                .HasForeignKey(movie => movie.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserMovieView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_UserMovies");
        });
    }
}
