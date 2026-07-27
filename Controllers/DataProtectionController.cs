using EventFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventFlow.Controllers;

public class DataProtectionController : Controller
{
    private readonly DataProtectionService _dataProtection;

    public DataProtectionController(DataProtectionService dataProtection)
    {
        _dataProtection = dataProtection;
    }

    public IActionResult Index()
    {
        var original = "EventFlow-Secret-Token";

        var encrypted = _dataProtection.Encrypt(original);

        var decrypted = _dataProtection.Decrypt(encrypted);

        ViewBag.Original = original;
        ViewBag.Encrypted = encrypted;
        ViewBag.Decrypted = decrypted;

        return View();
    }
}