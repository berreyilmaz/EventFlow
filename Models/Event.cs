using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models;

public class Event
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    // Organizer (Identity User)
    public string OrganizerId { get; set; } = string.Empty;

    public ApplicationUser Organizer { get; set; } = null!;

    public ICollection<Registration> Registrations
    {
        get;
        set;
    }
    = new List<Registration>();
}