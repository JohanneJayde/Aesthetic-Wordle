using AestheticWordle.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api.Controllers;

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

        GameResponseDto gameResponse = await GameService.PostGameResult(user, word, gameDto);

        return Ok(gameResponse);
    }

    [HttpPost("Guess")]
    [ProducesResponseType(typeof(GameStateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> ValidateGuess(GuessDto guess)
    {
        if (string.IsNullOrEmpty(guess.Guess) || guess.Guess.Length != 5)
        {
            return Ok(new GameStateDto()
            {
                IsWin = false,
                LetterStates = []
            });
        }

        bool isValid = await WordOfTheDayService.Exists(guess.Guess);

        if (!isValid)
        {
            return Ok(new GameStateDto()
            {
                IsWin = false,
                LetterStates = []
            });
        }

        Word? word = await WordOfTheDayService.GetWord(guess.WordId);

        if (word is null)
        {
            return NotFound("Word not found");
        }

        var state = GameService.ValidateGuess(guess.Guess, guess.AttemptNumber, word);

        return Ok(state);
    }

    [HttpPost("ValidateWord")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IsValidWord(string wordToValidate)
    {
        if(string.IsNullOrEmpty(wordToValidate) || wordToValidate.Length > 5)
        {
            return Ok(false);
        }

        bool isValid = await WordOfTheDayService.Exists(wordToValidate);

        if (!isValid)
        {
            return Ok(false);
        }

        return Ok(true);
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
