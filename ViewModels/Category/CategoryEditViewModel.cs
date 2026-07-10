using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels.Category;

public class CategoryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public string? Icon { get; set; }

    public bool IsActive { get; set; }
}