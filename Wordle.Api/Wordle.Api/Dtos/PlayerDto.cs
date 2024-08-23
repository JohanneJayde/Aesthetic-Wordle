namespace Wordle.Api.Dtos;

public class PlayerDto
{
    public AppUserDto User { get; set; } = null!;
    public int GameCount { get; set; }
    public double AverageAttempts { get; set; }
    public int AverageSeconds { get; set; }
}

