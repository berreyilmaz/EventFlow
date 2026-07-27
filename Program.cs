using EventFlow.Data;
using EventFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using EventFlow.Services;
using EventFlow.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Lockout
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(5);

        options.Lockout.MaxFailedAccessAttempts = 5;

        options.Lockout.AllowedForNewUsers = true;

        // Password Policy
        options.Password.RequiredLength = 8;

        options.Password.RequireDigit = true;

        options.Password.RequireUppercase = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireNonAlphanumeric = true;

        options.Password.RequiredUniqueChars = 3;
    })
    
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie Settings
    options.Cookie.Name = "EventFlow.Auth";

    options.Cookie.HttpOnly = true;

    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Strict;

    // Authentication
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/AccessDenied";

    // Session
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.SlidingExpiration = true;
});


// MVC
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "text/plain";

        await context.HttpContext.Response.WriteAsync(
            "Çok fazla istek gönderdiniz. Lütfen 1 dakika sonra tekrar deneyin.",
            cancellationToken);
    };

    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedRolesAsync(roleManager);
    await UserSeeder.SeedAdminAsync(userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    context.Response.Headers["Permissions-Policy"] =
        "camera=(), microphone=(), geolocation=()";

    context.Response.Headers["Content-Security-Policy"] =
    "default-src 'self'; " +
    "script-src 'self'; " +
    "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
    "font-src 'self' https://cdn.jsdelivr.net; " +
    "img-src 'self' data:;";

    await next();
});

app.UseRouting();

app.UseMiddleware<ExceptionLoggingMiddleware>();

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();