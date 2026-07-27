using EventFlow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers;

[Authorize]
public class ExceptionController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExceptionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var logs = await _context.ExceptionLogs
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return View(logs);
    }
}