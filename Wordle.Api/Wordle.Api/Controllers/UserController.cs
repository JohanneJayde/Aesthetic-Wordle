using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Dtos;
using Wordle.Api.Models;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

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

