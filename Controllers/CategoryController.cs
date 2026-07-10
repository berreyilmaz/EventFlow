using EventFlow.Data;
using EventFlow.Models;
using EventFlow.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Categories
            .Where(c => c.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                c.Name.Contains(search));
        }

        var categories = await query
            .OrderBy(c => c.Name)
            .ToListAsync();

        ViewBag.Search = search;

        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description,
            Icon = model.Icon
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        var model = new CategoryEditViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            IsActive = category.IsActive
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var category = await _context.Categories.FindAsync(model.Id);

        if (category == null)
            return NotFound();

        category.Name = model.Name;
        category.Description = model.Description;
        category.Icon = model.Icon;
        category.IsActive = model.IsActive;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Category model)
    {
        var category = await _context.Categories.FindAsync(model.Id);

        if (category == null)
            return NotFound();

        category.IsActive = false;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}