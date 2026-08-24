using Microsoft.EntityFrameworkCore;

namespace daper_54.Data;

/// <summary>Виконує SQL-код через EF Core після застосування міграцій.</summary>
public static class DatabaseInitializer
{
    public static async Task InitializeAsync(MovieDbContext context)
    {
        await context.Database.MigrateAsync();
        await context.Database.ExecuteSqlRawAsync(DatabaseSql.CreateUserMoviesView);
        await context.Database.ExecuteSqlRawAsync(DatabaseSql.CreateAddUserProcedure);
    }
}
