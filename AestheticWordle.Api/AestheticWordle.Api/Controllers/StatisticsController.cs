using Microsoft.AspNetCore.Mvc;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class StatisticsController(StatisticsService StatisticsService) : ControllerBase
{
    public StatisticsService StatisticsService { get; set; } = StatisticsService;


    [HttpPost("AllGamesStats")]
    public async Task<GameStatsDto> GetAllGameStats()
    {
        var stats = await StatisticsService.GetAllGames();

        return stats;
    }

    [HttpGet("Leaderboard")]
    public IEnumerable<PlayerDto> GetLeaderboard()
    {
        return StatisticsService.TopTenPlayers();
    }
}


