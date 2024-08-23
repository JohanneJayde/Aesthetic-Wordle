using Microsoft.AspNetCore.Identity;

namespace AestheticWordle.Api.Models;

public class AppUser : IdentityUser
{
    public int GameCount { get; set; }

    public double AverageAttempts { get; set; }

    public int AverageSecondsPerGame { get; set; }

    public ICollection<Game> Games { get; set; } = new List<Game>();
}

