using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels.Category;

public class CategoryCreateViewModel
{
    [Required(ErrorMessage ="Kategori adı zorunludur.")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public string? Icon { get; set; }
}