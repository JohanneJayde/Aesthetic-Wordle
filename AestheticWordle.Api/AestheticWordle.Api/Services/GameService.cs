using AestheticWordle.Api.Models;
using Microsoft.EntityFrameworkCore;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Models;

namespace AestheticWordle.Api.Services;
public class GameService(AppDbContext db)
{
    public AppDbContext Db { get; set; } = db;

    public async Task<GameResponseDto> PostGameResult(AppUser user, Word word, GameDto gameDto)
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

        user.GameCount++;

        user.AverageAttempts = ((user.AverageAttempts * (user.GameCount - 1)) + gameDto.Attempts) / user.GameCount;
        user.AverageSecondsPerGame = ((user.AverageSecondsPerGame * (user.GameCount - 1)) + gameDto.Seconds) / user.GameCount;

        await Db.SaveChangesAsync();

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

        return gameResponseDto;
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
                .ThenInclude(game => game.AppUser)
            .FirstOrDefaultAsync(wotd => wotd.Date == dateOnly);

        GameStatsDto stats = stats = new()
        {
            Date = dateOnly,
            AverageGuesses = 0,
            TotalTimesPlayed = 0,
            TotalWins = 0,
            AverageSeconds = 0,
            Users = []
        };

        if (word is not null && word.Games.Count != 0)
        {
            stats.Date = word!.Date;
            stats.AverageGuesses = word.Games.Average(g => g.Attempts);
            stats.TotalTimesPlayed = word.Games.Count;
            stats.TotalWins = word.Games.Count(g => g.IsWin);
            stats.AverageSeconds = word.Games.Average(w => w.Seconds);
            stats.Users = word.Games.Where(g => g.AppUser != null)
                            .GroupBy(g => g.AppUser!.Id)
                            .Select(group => group.First().AppUser)
                            .Select(user => new AppUserDto()
                            {
                                UserName = user!.UserName!,
                                Id = user!.Id,
                            }).ToList();
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

    public GameStateDto ValidateGuess(string guess, int attemptNumber, Word word)
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
                    letterState.State = LetterState.Wrong;
                }
            }
        }

        bool isWin = guessedWord.All(gs => gs.State == LetterState.Correct);

        return new GameStateDto()
        {
            LetterStates = guessedWord.Select(ls => ls.State).ToList(),
            IsWin = isWin,
            Solution = isWin || attemptNumber == 6 ? wordToGuess : null,
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
