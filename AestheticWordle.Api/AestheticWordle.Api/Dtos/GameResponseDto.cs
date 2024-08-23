namespace AestheticWordle.Api.Dtos;

public class GameResponseDto
{
    public int Id { get; set; }
    public int Attempts { get; set; }
    public int Seconds { get; set; }
    public bool IsWin { get; set; }
    public DateTime DateAttempted { get; set; } = DateTime.UtcNow;

    public string AppUserId { get; set; } = null!;

    public int WordId { get; set; }
}

