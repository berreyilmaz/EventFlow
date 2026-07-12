using System.ComponentModel.DataAnnotations;

namespace EventFlow.Models;

public class Registration
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    public DateTime RegisteredAt { get; set; }
        = DateTime.UtcNow;
}