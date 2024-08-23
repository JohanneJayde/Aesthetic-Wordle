using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Dtos;
using Wordle.Api.Identity;
using Wordle.Api.Models;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController(WordOfTheDayService wordOfTheDayService, WordEditorService wordEditorService) : ControllerBase
{
    [HttpGet("RandomWord")]
    public async Task<int> GetRandomWord()
    {
        Word randomWord = await wordOfTheDayService.GetRandomWord();

        return randomWord.WordId;
    }

    /// <summary>
    /// Get the word of the day.
    /// </summary>
    /// <param name="offsetInHours">Timezone offset in hours. Default to PST</param>
    /// <returns>wordId representing for the word of the day</returns>
    [HttpGet("WordOfTheDay")]
    public async Task<int> GetWordOfDay(double offsetInHours = -7.0)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(offsetInHours));

        Word wordOfTheDay = await wordOfTheDayService.GetWordOfTheDay(today);

        return wordOfTheDay.WordId;
    }

    [HttpGet("WordOfTheDay/{date}")]
    public async Task<int> GetWordOfDay(DateTime date)
    {
        DateOnly dateOnly = DateOnly.FromDateTime(date);

        Word wordOfTheDay = await wordOfTheDayService.GetWordOfTheDay(dateOnly);

        return wordOfTheDay.WordId;
    }

    [HttpGet("WordsList/")]
    public async Task<WordResultDto> GetWordsList(string query = "", int page = 1, int pageSize = 10)
    {
        return await wordOfTheDayService.GetWordsList(query, page, pageSize);
    }

    [HttpGet("ValidWordsList/")]
    public async Task<WordResultDto> GetValidWordsList(string query = "", int page = 1, int pageSize = 10)
    {
        return await wordOfTheDayService.GetWordsList(query, page, pageSize);
    }


    [HttpGet("FullWordsList/")]
    public async Task<WordResultDto> GetFullWordsList()
    {
        return await wordOfTheDayService.GetAllWords();
    }

    [HttpPost("AddWord")]
    [Authorize]
    public async Task AddWord(WordDto word)
    {
        await wordEditorService.AddWord(word);
    }

    [HttpDelete("DeleteWord")]
    [Authorize(Roles = Roles.Admin)]
    public async Task DeleteWord(string word)
    {
        await wordEditorService.DeleteWord(word);
    }

    [HttpPost("EditWord")]
    [Authorize(Roles = Roles.Admin)]
    public async Task EditWord(WordDto word)
    {
        await wordEditorService.UpdateWord(word);
    }
}
