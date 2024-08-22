using Microsoft.EntityFrameworkCore;
using Wordle.Api.Dtos;
using Wordle.Api.Models;

namespace Wordle.Api.Services;
public class StatisticsService(AppDbContext db)
{
    public AppDbContext Db { get; set; } = db;

    public async Task<GameStatsDto> GetAllGames()
    {
        IEnumerable<Game> games = await Db.Games.ToListAsync() ?? [];
        GameStatsDto stats;

        stats = new()
        {
            AverageGuesses = games.Average(g => g.Attempts),
            TotalTimesPlayed = games.Count(),
            TotalWins = games.Count(g => g.IsWin),
            AverageSeconds = games.Average(w => w.Seconds),
            Usernames = [.. games.Select(g => g.AppUser!.UserName).Where(name => !string.IsNullOrEmpty(name))]

        };

        return stats;
    }

    public IEnumerable<PlayerDto> TopTenPlayers()
    {
        return Db.Users.OrderBy(player => player.AverageAttempts)
            .ThenBy(player => player.GameCount)
            .ThenBy(player => player.AverageSecondsPerGame)
            .Select(player =>
            new PlayerDto(){ 
                Name = player.UserName!, 
                GameCount = player.GameCount, 
                AverageAttempts = player.AverageAttempts, 
                AverageSeconds = player.AverageSecondsPerGame })
            .Take(10);
    }
}

