using EventFlow.Data;
using EventFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ApplicationDbContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var log = new ExceptionLog
            {
                UserName = context.User.Identity?.Name ?? "Anonymous",
                ExceptionType = ex.GetType().Name,
                Message = ex.Message,
                Path = context.Request.Path,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            dbContext.ExceptionLogs.Add(log);
            await dbContext.SaveChangesAsync();

            context.Response.Redirect("/Home/Error");
        }
    }
}