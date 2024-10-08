﻿using System.ComponentModel.DataAnnotations;

namespace AestheticWordle.Api.Models;
public class Game
{
    public int GameId { get; set; }
    public int Attempts { get; set; }
    public int Seconds { get; set; }
    public bool IsWin { get; set; }

    public DateTime DateAttempted { get; set; } = DateTime.UtcNow;
    public int? WordOfTheDayId { get; set; }
    public WordOfTheDay? WordOfTheDay { get; set; }

    [Required]
    public string AppUserId { get; set; } = null!;
    public AppUser? AppUser { get; set; }

    [Required]
    public int WordId { get; set; }
    public Word? Word { get; set; }
}
