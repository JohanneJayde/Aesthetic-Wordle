namespace Wordle.Api.Dtos;

public class GameStateDto
{
    public List<WordState> WordStates { get; set; } = [];
    public bool IsWin { get; set; }
}
