using daper_54.Data;
using daper_54.Services;
using Microsoft.EntityFrameworkCore;

namespace daper_54;

internal class Program
{
    static async Task Main(string[] args)
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .Options;

        await using var context = new MovieDbContext(options);

        try
        { 
            await DatabaseInitializer.InitializeAsync(context);

            var repository = new MovieRepository(context);
            var userMovies = await repository.GetUserMoviesAsync();

            Console.WriteLine("База AC готова: view vw_UserMovies і процедура AddUser створені");
            foreach (var row in userMovies)
            {
                Console.WriteLine($"{row.Username}: {row.MovieTitle ?? "фільми ще не додані"}");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Не вдалося виконати ініціалізацію бази даних.");
            Console.WriteLine(exception.Message);
        }
    }
}
