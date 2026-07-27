using EventFlow.ViewModels.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlow.Controllers;

[Authorize]
public class SecurityLabsController : Controller
{
    public IActionResult Index()
    {
        return View(GetLabs());
    }

    public IActionResult Details(int id)
    {
        var lab = GetLabs().FirstOrDefault(x => x.Id == id);

        if (lab == null)
            return NotFound();

        return View(lab);
    }

    private List<SecurityLabViewModel> GetLabs()
    {
        return new()
        {
            new()
            {
                Id = 1,
                Title = "IDOR Protection",
                Category = "Authorization",
                Overview = "Prevent unauthorized access to resources owned by other users.",
                Vulnerability = "Users could manipulate identifiers to access resources they do not own.",
                Implementation = "Ownership validation was implemented before returning protected resources.",
                Testing = "Verified that another user's resources cannot be accessed."
            },

            new()
            {
                Id = 2,
                Title = "CSRF Protection",
                Category = "Request Security",
                Overview = "Prevent Cross-Site Request Forgery attacks.",
                Vulnerability = "Attackers could submit forged requests using an authenticated user's session.",
                Implementation = "Anti-forgery validation added to all POST requests.",
                Testing = "Requests without a valid anti-forgery token were rejected."
            },

            new()
            {
                Id = 3,
                Title = "Stored XSS Protection",
                Category = "Input Validation",
                Overview = "Prevent stored Cross-Site Scripting attacks.",
                Vulnerability = "Malicious JavaScript could execute in other users' browsers.",
                Implementation = "Razor HTML encoding was used for all user-generated content.",
                Testing = "Injected scripts were rendered as text."
            },

            new()
            {
                Id = 4,
                Title = "SQL Injection Prevention",
                Category = "Database Security",
                Overview = "Prevent SQL Injection attacks.",
                Vulnerability = "Dynamic SQL could expose the database.",
                Implementation = "Entity Framework Core parameterized queries were used.",
                Testing = "Injection payloads failed successfully."
            },

            new()
            {
                Id = 5,
                Title = "Secure File Upload",
                Category = "File Security",
                Overview = "Validate uploaded files.",
                Vulnerability = "Malicious executable files could be uploaded.",
                Implementation = "Validated extension, MIME type and file size.",
                Testing = "Invalid files were rejected."
            },

            new()
            {
                Id = 6,
                Title = "Brute Force Protection",
                Category = "Authentication",
                Overview = "Prevent password guessing attacks.",
                Vulnerability = "Unlimited login attempts.",
                Implementation = "Identity Lockout configured.",
                Testing = "Account locked after failed attempts."
            },

            new()
            {
                Id = 7,
                Title = "Rate Limiting",
                Category = "Network Security",
                Overview = "Limit excessive requests.",
                Vulnerability = "Unlimited requests may cause abuse.",
                Implementation = "ASP.NET Core Rate Limiter configured.",
                Testing = "HTTP 429 returned."
            },

            new()
            {
                Id = 8,
                Title = "HTTP Security Headers",
                Category = "Browser Security",
                Overview = "Protect browser communications.",
                Vulnerability = "Missing security headers.",
                Implementation = "Added standard security headers.",
                Testing = "Verified in browser DevTools."
            },

            new()
            {
                Id = 9,
                Title = "Content Security Policy",
                Category = "Browser Security",
                Overview = "Restrict external resources.",
                Vulnerability = "External scripts may execute.",
                Implementation = "Configured Content Security Policy.",
                Testing = "Unauthorized resources blocked."
            },

            new()
            {
                Id = 10,
                Title = "Audit Logging",
                Category = "Logging",
                Overview = "Track important user activities.",
                Vulnerability = "Critical actions not traceable.",
                Implementation = "Centralized audit logging service.",
                Testing = "Logs stored successfully."
            },

            new()
            {
                Id = 11,
                Title = "Authorization Logging",
                Category = "Logging",
                Overview = "Track forbidden access attempts.",
                Vulnerability = "Unauthorized requests were invisible.",
                Implementation = "Logged before returning Forbid().",
                Testing = "Unauthorized attempts recorded."
            },

            new()
            {
                Id = 12,
                Title = "Global Exception Logging",
                Category = "Monitoring",
                Overview = "Capture unhandled exceptions.",
                Vulnerability = "Unexpected errors lost.",
                Implementation = "Custom middleware logs exceptions.",
                Testing = "Exceptions stored in database."
            },

            new()
            {
                Id = 13,
                Title = "Secure Authentication Cookies",
                Category = "Authentication",
                Overview = "Secure authentication cookies.",
                Vulnerability = "Weak cookie configuration.",
                Implementation = "Configured HttpOnly, Secure and SameSite.",
                Testing = "Verified in browser."
            },

            new()
            {
                Id = 14,
                Title = "Strong Password Policy",
                Category = "Authentication",
                Overview = "Enforce strong passwords.",
                Vulnerability = "Weak passwords.",
                Implementation = "Identity password policy configured.",
                Testing = "Weak passwords rejected."
            },

            new()
            {
                Id = 15,
                Title = "Data Protection API",
                Category = "Encryption",
                Overview = "Encrypt sensitive data.",
                Vulnerability = "Plain text storage.",
                Implementation = "ASP.NET Core Data Protection API.",
                Testing = "Encryption and decryption successful."
            }
        };
    }
}