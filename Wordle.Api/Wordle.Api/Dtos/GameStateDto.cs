namespace Wordle.Api.Dtos;

public class GameStateDto
{
    public List<LetterState> LetterStates { get; set; } = [];
    public bool IsWin { get; set; }
}
