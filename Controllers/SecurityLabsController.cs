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

}