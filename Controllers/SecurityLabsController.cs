using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlow.Controllers;

[Authorize(Roles = "Admin")]
public class SecurityLabsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Csrf()
    {
        return View();
    }
}