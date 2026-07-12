using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EventFlow.ViewModels.Event;

public class EventCreateViewModel
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Range(1,10000)]
    public int Capacity { get; set; }

    public int CategoryId { get; set; }


    public IFormFile? Image { get; set; }
}