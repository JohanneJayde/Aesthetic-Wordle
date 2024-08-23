using AestheticWordle.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AestheticWordle.Api.Dtos;

namespace AestheticWordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    public AppDbContext _context = context;
    public UserManager<AppUser> _userManager = userManager;
    public RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] AccountRegistrationDto accountRegistrationInfo)
    {

        var user = await _userManager.FindByEmailAsync(accountRegistrationInfo.Email);

        if (user is not null)
        {
            return Conflict("Registration failed. Please check your information and try again.");
        }


        AppUser appUser = new AppUser()
        {
            UserName = accountRegistrationInfo.Username,
            Email = accountRegistrationInfo.Email,
        };

        var result = await _userManager.CreateAsync(appUser, accountRegistrationInfo.Password);

        if (result.Succeeded)
        {
            return Ok("Account successfully registered");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(errors);
        }

    }

}
