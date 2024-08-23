using Microsoft.AspNetCore.Mvc;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Models;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet("Get")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        UserResponseDto? user = await _userService.GetAppUserById(userId);

        if (user is null)
        {
            return NotFound("User found with " + userId);
        }

        return Ok(user);
    }

}

