using EventFlow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventFlow.Models;
using EventFlow.ViewModels.Event;
using EventFlow.Services;
using Microsoft.AspNetCore.Identity;

namespace EventFlow.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IAuditService _auditService;

    public EventController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment environment,
        IAuditService auditService)
    {
        _context = context;
        _userManager = userManager;
        _environment = environment;
        _auditService = auditService;
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
       string? imageName = null;

        if (model.Image != null)
        {

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (model.Image != null)
            {
                var extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image",
                        "Sadece JPG, JPEG ve PNG dosyaları yüklenebilir.");

                    ViewBag.Categories = new SelectList(
                        await _context.Categories
                            .Where(c => c.IsActive)
                            .OrderBy(c => c.Name)
                            .ToListAsync(),
                        "Id",
                        "Name");

                    return View(model);
                }
            }

            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    "Image",
                    "Dosya boyutu en fazla 2 MB olabilir.");

                ViewBag.Categories = new SelectList(
                    await _context.Categories
                        .Where(c => c.IsActive)
                        .OrderBy(c => c.Name)
                        .ToListAsync(),
                    "Id",
                    "Name");

                return View(model);
            }

            var allowedContentTypes = new[]
            {
                "image/jpeg",
                "image/png"
            };

            if (!allowedContentTypes.Contains(model.Image.ContentType))
            {
                ModelState.AddModelError(
                    "Image",
                    "Geçersiz dosya türü.");

                ViewBag.Categories = new SelectList(
                    await _context.Categories
                        .Where(c => c.IsActive)
                        .OrderBy(c => c.Name)
                        .ToListAsync(),
                    "Id",
                    "Name");

                return View(model);
            }

            if (!IsValidImage(model.Image))
            {
                ModelState.AddModelError(
                    "Image",
                    "Dosya içeriği geçerli bir resim değildir.");

                ViewBag.Categories = new SelectList(
                    await _context.Categories
                        .Where(c => c.IsActive)
                        .OrderBy(c => c.Name)
                        .ToListAsync(),
                    "Id",
                    "Name");

                return View(model);
            }

            imageName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);

            var uploadFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "events");

            var filePath = Path.Combine(uploadFolder, imageName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await model.Image.CopyToAsync(stream);
        }

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
            OrganizerId = organizerId!,
            ImageUrl = imageName,
        };

        _context.Events.Add(eventEntity);

        await _context.SaveChangesAsync();

        await _auditService.LogAsync(
        "Create Event",
        $"Created event '{eventEntity.Title}'",
        HttpContext);

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
            await _auditService.LogAsync(
                "Unauthorized Access",
                $"Attempted to edit event '{eventEntity.Title}'",
                HttpContext);

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
            await _auditService.LogAsync(
                "Unauthorized Access",
                $"Attempted to edit event '{eventEntity.Title}'",
                HttpContext);

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

        await _auditService.LogAsync(
        "Update Event",
        $"Updated event '{eventEntity.Title}'",
        HttpContext);

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
            await _auditService.LogAsync(
                "Unauthorized Access",
                $"Attempted to delete event '{eventEntity.Title}'",
                HttpContext);

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
            await _auditService.LogAsync(
                "Unauthorized Access",
                $"Attempted to delete event '{eventEntity.Title}'",
                HttpContext);

            return Forbid();
        }

        eventEntity.IsActive = false;

        await _context.SaveChangesAsync();

        await _auditService.LogAsync(
        "Delete Event",
        $"Deleted event '{eventEntity.Title}'",
        HttpContext);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
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

    private bool IsValidImage(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        Span<byte> header = stackalloc byte[8];
        stream.Read(header);

        // JPEG
        if (header[0] == 0xFF &&
            header[1] == 0xD8 &&
            header[2] == 0xFF)
        {
            return true;
        }

        // PNG
        if (header[0] == 0x89 &&
            header[1] == 0x50 &&
            header[2] == 0x4E &&
            header[3] == 0x47)
        {
            return true;
        }

        return false;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Join(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var eventEntity = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventEntity == null)
            return NotFound();

        // Kullanıcı zaten kayıtlı mı?
        if (eventEntity.Registrations.Any(r => r.UserId == userId))
        {
            TempData["Error"] = "Bu etkinliğe zaten kayıt oldunuz.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Kontenjan dolu mu?
        if (eventEntity.Registrations.Count >= eventEntity.Capacity)
        {
            TempData["Error"] = "Etkinliğin kontenjanı dolmuştur.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var registration = new Registration
        {
            EventId = id,
            UserId = userId
        };

        _context.Registrations.Add(registration);

        await _context.SaveChangesAsync();

        await _auditService.LogAsync(
        "Join Event",
        $"Joined event '{eventEntity.Title}'",
        HttpContext);

        TempData["Success"] = "Etkinliğe başarıyla kayıt oldunuz.";

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Leave(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var registration = await _context.Registrations
            .FirstOrDefaultAsync(r =>
                r.EventId == id &&
                r.UserId == userId);

        if (registration == null)
            return NotFound();

        var eventEntity = await _context.Events.FindAsync(id);

        if (eventEntity == null)
            return NotFound();

        _context.Registrations.Remove(registration);

        await _context.SaveChangesAsync();

        await _auditService.LogAsync(
        "Leave Event",
        $"Left event '{eventEntity.Title}'",
        HttpContext);

        TempData["Success"] = "Etkinlik kaydınız iptal edildi.";

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> MyRegistrations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var registrations = await _context.Registrations
            .Include(r => r.Event)
                .ThenInclude(e => e.Category)
            .Include(r => r.Event)
                .ThenInclude(e => e.Organizer)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RegisteredAt)
            .ToListAsync();

        return View(registrations);
    }
    }