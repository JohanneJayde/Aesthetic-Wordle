using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wordle.Api.Models;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<WordOfTheDay> WordsOfTheDays { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Word> Words { get; set; }
        
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

