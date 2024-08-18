using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wordle.Api.Models;

namespace Wordle.Api.Identity;

public static class IdentitySeed
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext db
    )
    {
        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Admin User
        await SeedAdminUserAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
        // Seed Roles
        if (!await roleManager.RoleExistsAsync(Roles.Awesome))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Awesome));
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
    {
        // Seed Admin User
        if (await userManager.FindByEmailAsync("Admin@AestheticWordle.com") == null)
        {
            AppUser user =
                new()
                {
                    UserName = "Admin@AestheticWordle.com",
                    Email = "Admin@AestheticWordle.com",
                };

            IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }

        if (await userManager.FindByEmailAsync("Awesome@AestheticWordle.com") == null)
        {
            AppUser user =
                new()
                {
                    UserName = "Awesome@AestheticWordle.com",
                    Email = "Awesome@AestheticWordle.com",
                };

            IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Awesome);
            }
        }
    }
}
