namespace AestheticWordle.Api.Dtos;

public class GuessDto
{
    public string Guess { get; set; } = null!;

    public int AttemptNumber { get; set; }

    public int WordId { get; set; }
}

