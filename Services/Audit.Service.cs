using EventFlow.Data;
using EventFlow.Models;

namespace EventFlow.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(
        string action,
        string description,
        HttpContext httpContext)
    {
        var log = new AuditLog
        {
            UserName = httpContext.User.Identity?.Name ?? "Anonymous",
            Action = action,
            Description = description,
            IpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);

        await _context.SaveChangesAsync();
    }
}