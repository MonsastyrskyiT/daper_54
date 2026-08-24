using System.ComponentModel.DataAnnotations;

namespace daper_54.Entities;

/// <summary>Фільм, доданий користувачем.</summary>
public class Movie
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ReleaseYear { get; set; }

    public string? Description { get; set; }
    public DateTime AddedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
