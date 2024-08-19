namespace Wordle.Api.Dtos;

    public class GuessDto
    {
        public string Guess { get; set; } = null!;
        public int WordId { get; set; }
    }

