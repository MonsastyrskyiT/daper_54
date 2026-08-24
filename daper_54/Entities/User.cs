using System.ComponentModel.DataAnnotations;

namespace daper_54.Entities;

/// <summary>Користувач платформи фільмів.</summary>
public class User
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;

    public ICollection<Movie> AddedMovies { get; set; } = new List<Movie>();
}
