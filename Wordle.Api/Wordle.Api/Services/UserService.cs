using Microsoft.EntityFrameworkCore;
using Wordle.Api.Dtos;
using Wordle.Api.Models;

namespace Wordle.Api.Services;

public class UserService(AppDbContext context)
{
    public async Task<UserResponseDto?> GetAppUserById(string userId)
    {
        var user = await context.Users
            .Include(user => user.Games)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null) return null;

        UserResponseDto userResponseDto = new()
        {
            Id = user.Id,
            UserName = user.UserName!,
            AverageAttempts = user.AverageAttempts,
            GameCount = user.GameCount,
            AverageSecondsPerGame = user.AverageSecondsPerGame,
            Games = user.Games.Select(g => new GameResponseDto()
            {
                Id = g.GameId,
                Attempts = g.Attempts,
                Seconds = g.Seconds,
                WordId = g.WordId,
                AppUserId = g.AppUserId,
                DateAttempted = g.DateAttempted,
                IsWin = g.IsWin,
            }).OrderByDescending(gr => gr.DateAttempted).ToList()
        };

        return userResponseDto;
    }
}

