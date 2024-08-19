using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Dtos;
using Wordle.Api.Models;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class GameController(GameService gameService, WordOfTheDayService wordofTheDayService, UserManager<AppUser> userManager) : ControllerBase
{
    public GameService GameService { get; set; } = gameService;
    public WordOfTheDayService WordOfTheDayService { get; set; } = wordofTheDayService;
    public UserManager<AppUser> UserManager { get; set; } = userManager;

    [HttpPost("Result")]
    [Authorize]
    public async Task<IActionResult> PostGame(GameDto gameDto)
    {
        Word? word = await WordOfTheDayService.GetWord(gameDto.WordId);

        if (word is null)
        {
            return BadRequest("Word not found");
        }

        AppUser? user = await UserManager.GetUserAsync(User);

        if (user is null)
        {
            return BadRequest("An error occurred while processing your request.");
        }

        Game game = await GameService.PostGameResult(user, word, gameDto);
        var stats = await GameService.GetGameStats(game);

        return Ok(stats);
    }

    [HttpPost("Guess")]
    [ProducesResponseType(typeof(GameStateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateGuess(GuessDto guess)
    {
        Word? word = await WordOfTheDayService.GetWord(guess.WordId);

        if (word is null)
        {
            return BadRequest("Word not found");
        }

        var state = GameService.ValidateGuess(guess.Guess, word);

        return Ok(state);
    }

    [HttpGet("WordOfTheDayStats/{date}")]
    public async Task<GameStatsDto> GetWordOfTheDayStats(DateTime date)
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
