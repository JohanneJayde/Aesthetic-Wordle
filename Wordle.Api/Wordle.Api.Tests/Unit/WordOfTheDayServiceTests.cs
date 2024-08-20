using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wordle.Api.Dtos;
using Wordle.Api.Models;

namespace Wordle.Api.Tests.Unit;

[TestClass]
public class WordOfTheDayServiceTests : DatabaseTestBase
{
    public WordOfTheDayService WordOfTheDayService { get; set; } = null!;

    [TestInitialize]
    public async Task Setup()
    {
        InitializeDb();

        using (var context = new AppDbContext(Options))
        {
            context.Words.AddRange(
                new Word { Text = "apple" },
                new Word { Text = "blank" },
                new Word { Text = "clear" },
                new Word { Text = "snips" }

            );

            await context.SaveChangesAsync();
        }

        WordOfTheDayService = new WordOfTheDayService(new AppDbContext(Options));
    }

    [DataTestMethod]
    [DataRow("cubur", true)]
    [DataRow("chaur", true)]
    [DataRow("chalk", false)]
    [DataRow("floor", false)]

    public async Task CheckForCorrectLetters_FirstPositionCorrectAtWord_ReturnsExpectedResult(string wordToCheck, bool expected)
    {
        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "clear");
        }

        List<string> guesses = ["czzzr", "anvil", "artsy", "value", "callr"];

        bool result = WordOfTheDayService.CheckForCorrectLetters(wordToCheck, guesses, word.Text);

        result.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("cubur", true)]
    [DataRow("chaur", true)]
    [DataRow("chalk", false)]
    [DataRow("floor", false)]
    [DataRow("alcer", false)]

    public async Task CheckForWrongLetters_FirstPositionCorrectAtWord_ReturnsExpectedResult(string wordToCheck, bool expected)
    {
        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "clear");
        }

        List<string> guesses = ["czzzr", "anvil", "artsy", "value", "callr"];

        bool result = WordOfTheDayService.CheckForWrongLetters(wordToCheck, guesses, word.Text);

        result.Should().Be(expected);
    }

    [TestCleanup]
    public void Cleanup()
    {
        CloseDbConnection();
    }

}

