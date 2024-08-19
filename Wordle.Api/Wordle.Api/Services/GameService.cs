using Microsoft.EntityFrameworkCore;
using Wordle.Api.Dtos;
using Wordle.Api.Models;

namespace Wordle.Api.Services;
public class GameService(AppDbContext db)
{
    public AppDbContext Db { get; set; } = db;

    public async Task<Game> PostGameResult(AppUser user, Word word, GameDto gameDto)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        Game game = new()
        {
            Attempts = gameDto.Attempts,
            IsWin = gameDto.IsWin,
            WordOfTheDay = word.WordsOfTheDays
                .OrderByDescending(wotd => wotd.Date)
                .FirstOrDefault(wotd => wotd.Date <= today),
            Word = word,
            Seconds = gameDto.Seconds,
            AppUser = user,
        };

        Db.Games.Add(game);
        await Db.SaveChangesAsync();
        return game;
    }

    public async Task<GameStatsDto> GetGameStats(Game game)
    {
        var gamesForWord = Db.Games.Where(g => g.WordId == game.WordId);

        GameStatsDto stats = new()
        {
            Word = game.Word!.Text,
            AverageGuesses = await gamesForWord.AverageAsync(g => g.Attempts),
            TotalTimesPlayed = await gamesForWord.CountAsync(),
            AverageSeconds = await gamesForWord.AverageAsync(g => g.Seconds),
            TotalWins = await gamesForWord.CountAsync(g => g.IsWin),
            Usernames = [.. gamesForWord.Select(g => g.AppUser!.UserName).Where(name => !string.IsNullOrEmpty(name))]
        };

        return stats;
    }

    public IQueryable<AllWordStats> StatsForAllWords()
    {
        IQueryable<AllWordStats> result = Db.Games
            .Include(g => g.Word)
            .GroupBy(g => g.Word!.Text)
            .Select(g => new AllWordStats()
            {
                Word = g.Key,
                AverageGuesses = g.Average(x => x.Attempts)
            });

        return result;
    }

    public async Task<GameStatsDto> WordOfDayStats(DateTime date)
    {
        DateOnly dateOnly = DateOnly.FromDateTime(date);

        WordOfTheDay? word = await Db.WordsOfTheDays
            .Include(wotd => wotd.Games)
            .FirstOrDefaultAsync(wotd => wotd.Date == dateOnly);

        IEnumerable<Game> wordOfTheDayGames;
        GameStatsDto stats = stats = new()
        {
            Date = dateOnly,
            AverageGuesses = 0,
            TotalTimesPlayed = 0,
            TotalWins = 0,
            AverageSeconds = 0,
            Usernames = []
        };

        if (word is not null && word.Games.Count != 0)
        {
            wordOfTheDayGames = word.Games;

            stats.Date = word!.Date;
            stats.AverageGuesses = wordOfTheDayGames.Average(g => g.Attempts);
            stats.TotalTimesPlayed = wordOfTheDayGames.Count();
            stats.TotalWins = wordOfTheDayGames.Count(g => g.IsWin);
            stats.AverageSeconds = wordOfTheDayGames.Average(w => w.Seconds);
            stats.Usernames = [.. wordOfTheDayGames.Select(g => g.AppUser!.UserName).Where(name => !string.IsNullOrEmpty(name))];
        }

        return stats;
    }


    public async Task<List<GameStatsDto>> LastTenWordStats(DateTime date)
    {

        List<GameStatsDto> wordOfTheDayGames = []; ;

        for (int i = 0; i < 10; i++)
        {
            wordOfTheDayGames.Add(await WordOfDayStats(date.AddDays(-i)));
        }
        return wordOfTheDayGames;
    }

    public GameStateDto ValidateGuess(string guess, Word word)
    {
        var guessedWord = guess.Select(letter => new GuessedLetterState()
        {
            Letter = char.ToLower(letter),
            State = LetterState.Unknown

        }).ToArray();

        string wordToGuess = word.Text.ToLower();

        Dictionary<char, int> letterCounts = wordToGuess
            .GroupBy(letter => letter)
            .ToDictionary(group => group.Key, group => group.Count());

        for (int i = 0; i < guessedWord.Length; i++)
        {
            if (guessedWord[i].Letter == wordToGuess[i])
            {
                letterCounts[guessedWord[i].Letter]--;
                guessedWord[i].State = LetterState.Correct;
            }
        }

        foreach (var letterState in guessedWord)
        {
            if (letterState.State == LetterState.Unknown)
            {
                foreach (char letter in wordToGuess)
                {
                    if (letterState.Letter == letter && letterCounts[letter] > 0)
                    {
                        letterState.State = LetterState.Misplaced;
                        letterCounts[letter]--;
                    }
                }
                if (letterState.State == LetterState.Unknown)
                {
                    letterState.State = LetterState.Incorrect;
                }
            }
        }

        bool isWin = guessedWord.All(gs => gs.State == LetterState.Correct);

        return new GameStateDto()
        {
            LetterStates = guessedWord.Select(ls => ls.State).ToList(),
            IsWin = isWin
        };
    }

    public class AllWordStats()
    {
        public required string Word { get; set; }

        public double AverageGuesses { get; set; }
    }

    public class GuessedLetterState()
    {
        public char Letter { get; set; }
        public LetterState State { get; set; }
    }
}
