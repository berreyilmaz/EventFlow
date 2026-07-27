using Microsoft.AspNetCore.Http;

namespace EventFlow.Services;

public interface IAuditService
{
    Task LogAsync(
        string action,
        string description,
        HttpContext httpContext);
}