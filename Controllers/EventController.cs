using EventFlow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventFlow.Models;
using EventFlow.ViewModels.Event;

namespace EventFlow.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();

        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id",
                "Name");

            return View(model);
        }

        var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var eventEntity = new Event
        {
            Title = model.Title,
            Description = model.Description,
            Location = model.Location,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Capacity = model.Capacity,
            CategoryId = model.CategoryId,
            OrganizerId = organizerId!
        };

        _context.Events.Add(eventEntity);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var events = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        return View(events);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var eventEntity = await _context.Events.FindAsync(id);

        if (eventEntity == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            eventEntity.OrganizerId != currentUserId)
        {
            return Forbid();       
        }

        ViewBag.Categories = new SelectList(
            await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync(),
            "Id",
            "Name",
            eventEntity.CategoryId);

        var model = new EventEditViewModel
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            Location = eventEntity.Location,
            StartDate = eventEntity.StartDate,
            EndDate = eventEntity.EndDate,
            Capacity = eventEntity.Capacity,
            CategoryId = eventEntity.CategoryId
        };

        return View(model);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EventEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id",
                "Name",
                model.CategoryId);

            return View(model);
        }

        var eventEntity = await _context.Events.FindAsync(model.Id);

        if (eventEntity == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            eventEntity.OrganizerId != currentUserId)
        {
            return Forbid();
        }

        eventEntity.Title = model.Title;
        eventEntity.Description = model.Description;
        eventEntity.Location = model.Location;
        eventEntity.StartDate = model.StartDate;
        eventEntity.EndDate = model.EndDate;
        eventEntity.Capacity = model.Capacity;
        eventEntity.CategoryId = model.CategoryId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventEntity == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            eventEntity.OrganizerId != currentUserId)
        {
            return Forbid();
        }

        return View(eventEntity);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var eventEntity = await _context.Events.FindAsync(id);

        if (eventEntity == null)
            return NotFound();

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!User.IsInRole("Admin") &&
            eventEntity.OrganizerId != currentUserId)
        {
            return Forbid();
        }

        eventEntity.IsActive = false;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

        if (eventEntity == null)
            return NotFound();

        return View(eventEntity);
    }

    [HttpGet]
    public async Task<IActionResult> MyEvents()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var events = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .Where(e => e.IsActive &&
                        e.OrganizerId == currentUserId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        return View(events);
    }

    [HttpGet]
    public async Task<IActionResult> SqlSearch(string? search)
    {
        var events = await _context.Events
            .Where(e =>
                string.IsNullOrEmpty(search) ||
                e.Title.Contains(search))
            .ToListAsync();

        ViewBag.Search = search;

        return View(events);
    }

    }