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
            TotalCategories = await _context.Categories.CountAsync(c => c.IsActive),

            TotalEvents = await _context.Events.CountAsync(e => e.IsActive),

            TotalUsers = await _context.Users.CountAsync(),

            RecentEvents = await _context.Events
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}