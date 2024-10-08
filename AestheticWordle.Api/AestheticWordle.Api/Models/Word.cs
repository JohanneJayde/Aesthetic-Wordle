﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AestheticWordle.Api.Models;

[Index(nameof(Text), IsUnique = true)]
public class Word
{
    public int WordId { get; set; }

    [Required]
    public required string Text { get; set; }

    public ICollection<Game> Games { get; set; } = [];
    public ICollection<WordOfTheDay> WordsOfTheDays { get; set; } = [];

    public bool IsCommonWord { get; set; } = true;
}
