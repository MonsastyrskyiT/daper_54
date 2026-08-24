using daper_54.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace daper_54.Data;

public class MovieDbContextFactory : IDesignTimeDbContextFactory<MovieDbContext>
{
    public MovieDbContext CreateDbContext(string[] args)
    {
        var connectionString = ConnectionStringProvider.Load(
            Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new MovieDbContext(options);
    }
}
