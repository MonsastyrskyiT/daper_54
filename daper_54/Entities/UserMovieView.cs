using Microsoft.EntityFrameworkCore;

namespace daper_54.Entities;

[Keyless]
public class UserMovieView
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? MovieId { get; set; }
    public string? MovieTitle { get; set; }
    public int? ReleaseYear { get; set; }
    public DateTime? AddedAt { get; set; }
}
