using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wordle.Api.Dtos;
using Wordle.Api.Models;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class GameController(GameService gameService, UserManager<AppUser> userManager) : ControllerBase
{
    public GameService GameService { get; set; } = gameService;
    UserManager<AppUser> UserManager { get; set; } = userManager;

    [HttpPost("Result")]
    public async Task<IActionResult> PostGame(GameDto gameDto)
    {
        var email = User.FindFirstValue("email");

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("Cannot save for unauthorized User");
        }

        AppUser? user = await UserManager.FindByEmailAsync(email);

        if(user is null)
        {
            return BadRequest("An error occurred while processing your request.");
        }

        Game game = await GameService.PostGameResult(user, gameDto);
        var stats = await GameService.GetGameStats(game);

        return Ok(stats);
    }

    [HttpGet("WordOfTheDayStats/{date}")]
    public async Task<GameStatsDto> GetWordOfTheDayStats (DateTime date)
    {
        var stats = await GameService.WordOfDayStats(date);

        return stats;
    }

    [HttpGet("LastTenWordOfTheDayStats/{date}")]
    public async Task<List<GameStatsDto>> GetLastTenDayWordStats(DateTime date)
    {
        var stats = await GameService.LastTenWordStats(date);

        return stats;
    }

}
