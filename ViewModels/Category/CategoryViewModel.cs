using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels;

public class CategoryViewModel
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public string? Icon { get; set; }
}