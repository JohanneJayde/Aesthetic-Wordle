using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Models;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api.Tests.Unit;

[TestClass]
public class GameServiceTests : DatabaseTestBase
{
    public GameService GameService { get; set; } = null!;

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

        GameService = new GameService(new AppDbContext(Options));
    }

    [DataTestMethod]
    [DataRow("zzzzz")]
    public async Task ValidateGuess_NoCorrectLetters_ReturnsAllIncorrectStates(string guess)
    {

        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "apple");
        }

        Assert.IsNotNull(word);

        var state = GameService.ValidateGuess(guess, 3, word);

        Assert.IsNotNull(state);
        state.LetterStates[0].Should().Be(LetterState.Wrong);
        state.IsWin.Should().BeFalse();
    }

    [DataTestMethod]
    [DataRow("apple")]
    public async Task ValidateGuess_AllCorrectLetters_ReturnsAllIncorrectStates(string guess)
    {

        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "apple");
        }

        var state = GameService.ValidateGuess(guess, 4, word);

        Assert.IsNotNull(state);
        state.LetterStates.Should().OnlyContain(state => state == LetterState.Correct);
        state.IsWin.Should().BeTrue();

    }

    [DataTestMethod]
    [DataRow("balco")]
    public async Task ValidateGuess_MismatchLetters_ReturnsExpectedStates(string guess)
    {
        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "clear");
        }

        GameStateDto state = GameService.ValidateGuess(guess, 2, word);

        state.LetterStates[0].Should().Be(LetterState.Wrong);
        state.LetterStates[1].Should().Be(LetterState.Misplaced);
        state.LetterStates[2].Should().Be(LetterState.Misplaced);
        state.LetterStates[3].Should().Be(LetterState.Misplaced);
        state.LetterStates[4].Should().Be(LetterState.Wrong);

        state.IsWin.Should().BeFalse();

    }

    [DataTestMethod]
    [DataRow("swiss")]
    public async Task ValidateGuess_DuplicateLetters_ReturnsExpectedStates(string guess)
    {
        Word word;

        using (var context = new AppDbContext(Options))
        {
            word = await context.Words.FirstAsync(w => w.Text == "snips");
        }

        GameStateDto state = GameService.ValidateGuess(guess, 3, word);

        state.LetterStates[0].Should().Be(LetterState.Correct);
        state.LetterStates[1].Should().Be(LetterState.Wrong);
        state.LetterStates[2].Should().Be(LetterState.Correct);
        state.LetterStates[3].Should().Be(LetterState.Wrong);
        state.LetterStates[4].Should().Be(LetterState.Correct);

        state.IsWin.Should().BeFalse();

    }

    [TestCleanup]
    public void Cleanup()
    {
        CloseDbConnection();
    }

}

