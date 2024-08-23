using Microsoft.EntityFrameworkCore;
using AestheticWordle.Api.Dtos;
using AestheticWordle.Api.Models;

namespace AestheticWordle.Api.Services;

public class WordEditorService(AppDbContext Db)
{
    public AppDbContext Db { get; set; } = Db;

    public async Task DeleteWord(string Word)
    {
        Word? word = await Db.Words.FirstOrDefaultAsync(word => word.Text == Word);

        if (word is not null)
        {
            Db.Words.Remove(word);
            await Db.SaveChangesAsync();
        }

    }

    public async Task UpdateWord(WordDto wordToEdit)
    {
        Word? word = await Db.Words.FirstOrDefaultAsync(word => word.Text == wordToEdit.Word);

        if (word is not null)
        {
            word.IsCommonWord = wordToEdit.IsCommonWord;

            await Db.SaveChangesAsync();
        }
    }

    public async Task AddWord(WordDto wordToAdd)
    {
        if (wordToAdd.Word.Length != 5)
        {
            return;
        }

        Word word = new Word()
        {
            Text = wordToAdd.Word,
            IsCommonWord = wordToAdd.IsCommonWord
        };

        Db.Words.Add(word);

        await Db.SaveChangesAsync();
    }
}

