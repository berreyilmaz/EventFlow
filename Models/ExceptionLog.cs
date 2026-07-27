namespace EventFlow.Models;

public class ExceptionLog
{
    public int Id { get; set; }

    public string UserName { get; set; } = "Anonymous";

    public string ExceptionType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}