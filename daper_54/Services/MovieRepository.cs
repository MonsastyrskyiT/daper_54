using System.Data;
using System.Data.Common;
using Dapper;
using daper_54.Data;
using daper_54.Entities;
using Microsoft.EntityFrameworkCore;

namespace daper_54.Services;

public class MovieRepository
{
    private readonly MovieDbContext _context;

    public MovieRepository(MovieDbContext context)
    {
        _context = context;
    }

    public Task<int> AddUserWithProcedureAsync(User user) =>
        UseConnectionAsync(connection => connection.QuerySingleAsync<int>(
            "dbo.AddUser",
            new { user.Username, user.Email, user.Password },
            commandType: CommandType.StoredProcedure));
    public async Task<List<UserMovieView>> GetUserMoviesAsync()
    {
        const string sql = """
            SELECT UserId, Username, Email, MovieId, MovieTitle, ReleaseYear, AddedAt
            FROM dbo.vw_UserMovies
            ORDER BY Username, MovieTitle;
            """;

        return (await UseConnectionAsync(connection =>
            connection.QueryAsync<UserMovieView>(sql))).ToList();
    }

    public async Task<int> AddMovieAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return movie.Id;
    }

    public Task<int> UpdateMovieDescriptionWithSqlAsync(int movieId, string? description) =>
        _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE dbo.Movies SET Description = {description} WHERE Id = {movieId};");

    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie is null)
        {
            return false;
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<T> UseConnectionAsync<T>(Func<DbConnection, Task<T>> action)
    {
        var connection = _context.Database.GetDbConnection();
        var mustClose = connection.State != ConnectionState.Open;
        if (mustClose)
        {
            await connection.OpenAsync();
        }

        try
        {
            return await action(connection);
        }
        finally
        {
            if (mustClose)
            {
                await connection.CloseAsync();
            }
        }
    }
}
