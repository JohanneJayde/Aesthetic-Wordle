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

    [HttpPost("SaveResult")]
    [Authorize]
    [ProducesResponseType(typeof(GameResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]

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
            return Unauthorized("User not found");
        }

        Game game = await GameService.PostGameResult(user, word, gameDto);

        GameResponseDto gameResponseDto = new()
        {
            Id = game.GameId,
            Attempts = game.Attempts,
            Seconds = game.Seconds,
            IsWin = game.IsWin,
            DateAttempted = game.DateAttempted,
            AppUserId = game.AppUserId,
            WordId = game.WordId,
        };

        return Ok(gameResponseDto);
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

        var state = GameService.ValidateGuess(guess.Guess, guess.AttemptNumber, word);

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
