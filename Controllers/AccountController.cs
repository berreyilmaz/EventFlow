using EventFlow.Models;
using EventFlow.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EventFlow.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    

public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("Login");
        }

        if (result.Succeeded)
        {
            return RedirectToAction("Login");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    

    [HttpGet]
    [EnableRateLimiting("fixed")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(
                "",
                "Hesabınız çok fazla başarısız giriş nedeniyle 5 dakika kilitlendi.");

            return View(model);
        }

        ModelState.AddModelError(
            "",
            "Kullanıcı adı veya şifre hatalı.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}