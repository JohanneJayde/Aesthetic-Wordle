using AestheticWordle.Api.Models;
using AestheticWordle.Api.Services;

namespace AestheticWordle.Api.Tests.Unit;

[TestClass]
public class WordOfTheDayServiceTests : DatabaseTestBase
{
    [TestMethod]
    public void LoadWordList_SuccessfullyGetsWords()
    {
        CollectionAssert.AllItemsAreNotNull(WordOfTheDayService.WordList());
    }

    [TestMethod]
    public void GetWordOfTheDay_ReturnsString()
    {
        CollectionAssert.Contains(WordOfTheDayService.WordList(), "yules");
    }

    [TestMethod]
    public void GetWordOfTheDay_SameWord()
    {
        // Arrange
        using var context = new AppDbContext(Options);
        WordOfTheDayService service = new(context);
        DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);

        // Act
        var word = service.GetWordOfTheDay(date);

        // Assert
        Equals(word, service.GetWordOfTheDay(date));
    }
}