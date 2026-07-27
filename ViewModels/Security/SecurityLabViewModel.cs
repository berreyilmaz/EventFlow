namespace EventFlow.ViewModels.Security;

public class SecurityLabViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Overview { get; set; } = string.Empty;

    public string Vulnerability { get; set; } = string.Empty;

    public string Implementation { get; set; } = string.Empty;

    public string Testing { get; set; } = string.Empty;

    public bool Completed { get; set; } = true;
}