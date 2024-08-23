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
            Users = games.Where(g => g.AppUser != null)
                            .GroupBy(g => g.AppUser!.Id)
                            .Select(group => group.First().AppUser)
                            .Select(user => new AppUserDto()
                            {
                                UserName = user!.UserName!,
                                Id = user!.Id,
                            }).ToList()

        };

        return stats;
    }

    public IEnumerable<PlayerDto> TopTenPlayers()
    {
        return Db.Users.Where(u => u.GameCount > 0)
            .OrderBy(player => player.AverageAttempts)
            .ThenBy(player => player.GameCount)
            .ThenBy(player => player.AverageSecondsPerGame)
            .Select(player =>
            new PlayerDto()
            {
                User = new AppUserDto()
                {
                    UserName = player.UserName!,
                    Id = player.Id
                },
                GameCount = player.GameCount,
                AverageAttempts = player.AverageAttempts,
                AverageSeconds = player.AverageSecondsPerGame
            })
            .Take(10);
    }
}

