using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels.Event;

public class EventEditViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public int CategoryId { get; set; }
}