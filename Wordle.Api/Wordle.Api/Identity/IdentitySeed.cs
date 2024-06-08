﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wordle.Api.Models;

namespace Wordle.Api.Identity;
public static class IdentitySeed
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, WordleDbContext db)
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
        if (await userManager.FindByEmailAsync("Admin@intellitect.com") == null)
        {

            AppUser user = new AppUser
            {
                UserName = "Admin@intellitect.com",
                Email = "Admin@intellitect.com",
                Birthday = new DateTime(2007, 5, 6)
            };

            IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }

        if (await userManager.FindByEmailAsync("Awesome@intellitect.com") == null)
        {
            AppUser user = new AppUser
            {
                UserName = "Awesome@intellitect.com",
                Email = "Awesome@intellitect.com",
                Birthday = new DateTime(2000, 10, 31)
            };

            IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;
            var masterOfTheUniverseClaim = new Claim(Claims.MasterOfTheUniverse, "true");

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, masterOfTheUniverseClaim);
                await userManager.AddToRoleAsync(user, Roles.Awesome);
            }
        }
    }
}