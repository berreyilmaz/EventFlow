using EventFlow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers;

[Authorize(Roles = "Admin")]
public class SecurityLabsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SecurityLabsController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Idor()
    {
        return View();
    }

    public IActionResult Csrf()
    {
        return View();
    }

    public IActionResult Xss()
    {
        return View();
    }

    public IActionResult SqlInjection()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> SqlInjectionUnsafe(string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return View(new List<EventFlow.Models.Event>());

        using var connection = _context.Database.GetDbConnection();

        await connection.OpenAsync();

        var command = connection.CreateCommand();

        // ⚠️ Bilerek güvensiz
        command.CommandText =
            @"SELECT Id, Title, Description, Location, StartDate, EndDate,
            Capacity, CategoryId, OrganizerId, IsActive, CreatedAt
            FROM Events
            WHERE Title = @title";

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@title";
        parameter.Value = search;

        command.Parameters.Add(parameter);

        var reader = await command.ExecuteReaderAsync();

        var events = new List<EventFlow.Models.Event>();

        while (await reader.ReadAsync())
        {
            events.Add(new EventFlow.Models.Event
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Location = reader.GetString(3),
                StartDate = reader.GetDateTime(4),
                EndDate = reader.GetDateTime(5),
                Capacity = reader.GetInt32(6),
                CategoryId = reader.GetInt32(7),
                OrganizerId = reader.GetString(8),
                IsActive = reader.GetBoolean(9),
                CreatedAt = reader.GetDateTime(10)
            });
        }

        ViewBag.Search = search;

        return View(events);
    }
}