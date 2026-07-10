using EventFlow.Models;
using Microsoft.AspNetCore.Identity;

namespace EventFlow.Data;

public static class UserSeeder
{
    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@eventflow.com";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin != null)
            return;

        admin = new ApplicationUser
        {
            FullName = "System Administrator",
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}