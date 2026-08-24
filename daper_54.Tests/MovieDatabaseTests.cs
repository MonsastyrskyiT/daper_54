using System.ComponentModel.DataAnnotations;
using daper_54.Data;
using daper_54.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace daper_54.Tests;

public class MovieDatabaseTests
{
    [Fact]
    public void User_requires_a_valid_email()
    {
        var user = new User { Username = "anna", Email = "wrong-email", Password = "secret" };
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, result => result.MemberNames.Contains(nameof(User.Email)));
    }

    [Fact]
    public void Username_has_a_unique_index()
    {
        using var context = CreateContext();
        var entityType = context.Model.FindEntityType(typeof(User));
        var index = Assert.Single(entityType!.GetIndexes(), index =>
            index.Properties.Single().Name == nameof(User.Username));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void User_movies_is_a_keyless_view()
    {
        using var context = CreateContext();
        var viewType = context.Model.FindEntityType(typeof(UserMovieView));

        Assert.Null(viewType!.FindPrimaryKey());
        Assert.Equal("vw_UserMovies", viewType.GetViewName());
    }

    [Fact]
    public void Sql_defines_view_and_add_user_procedure()
    {
        Assert.Contains("CREATE OR ALTER VIEW dbo.vw_UserMovies", DatabaseSql.CreateUserMoviesView);
        Assert.Contains("CREATE OR ALTER PROCEDURE dbo.AddUser", DatabaseSql.CreateAddUserProcedure);
    }

    private static MovieDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MovieDbContext(options);
    }
}
