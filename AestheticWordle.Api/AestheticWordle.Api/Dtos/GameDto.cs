namespace AestheticWordle.Api.Dtos;

public class GameDto
{
    public int Attempts { get; set; }
    public int Seconds { get; set; }

    public bool IsWin { get; set; }

    public int WordId { get; set; }
}

