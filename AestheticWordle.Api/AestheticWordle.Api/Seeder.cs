using AestheticWordle.Api.Models;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api;
public static class Seeder
{
    public static async Task Seed(AppDbContext db)
    {
        if (!db.Words.Any())
        {
            foreach (var word in WordOfTheDayService.WordList())
            {
                db.Words.Add(new Word() { Text = word });
            }

            await db.SaveChangesAsync();
        }
    }
}
