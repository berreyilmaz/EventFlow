using EventFlow.Data;
using EventFlow.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalUsers = await _context.Users.CountAsync(),

            TotalEvents = await _context.Events
                .Where(x => x.IsActive)
                .CountAsync(),

            TotalRegistrations = await _context.Registrations.CountAsync(),

            TotalAuditLogs = await _context.AuditLogs.CountAsync(),

            UnauthorizedAttempts = await _context.AuditLogs
                .CountAsync(x => x.Action == "Unauthorized Access"),

            TotalExceptions = await _context.ExceptionLogs.CountAsync()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult TestException()
    {
        throw new Exception("This is a test exception.");
    }
}