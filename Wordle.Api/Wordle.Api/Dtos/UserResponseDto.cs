namespace Wordle.Api.Dtos;

public class UserResponseDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public int GameCount { get; set; }
    public double AverageAttempts { get; set; }
    public int AverageSecondsPerGame { get; set; }
    public List<GameResponseDto> Games { get; set; } = [];
}

