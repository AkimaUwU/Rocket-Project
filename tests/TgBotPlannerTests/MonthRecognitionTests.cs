using TgBotPlannerTests.Models;

namespace TgBotPlannerTests;

public class MonthRecognitionTests
{
    private readonly MonthRecognizer _recognizer;

    public MonthRecognitionTests()
    {
        _recognizer = new MonthRecognizer();
    }

    [Fact]
    public async Task Test_Recognition_ValidMonths()
    {
        string[] inputs = new[]
        {
            "январ",
            "феврал",
            "март",
            "апрел",
            "май",
            "мая",
            "маю",
            "июн",
            "июл",
            "август",
            "сентябр",
            "октябр",
            "ноябр",
            "декабр",
        };

        string[] expectedResults = new[]
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "5",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
        };

        for (int i = 0; i < inputs.Length; i++)
        {
            RecognitionResult result = await _recognizer.TryRecognize(inputs[i]);
            Assert.True(result.IsRecognized);
            Assert.Equal(expectedResults[i], result.RecognizedMetadata);
        }
    }

    [Fact]
    public async Task Test_Recognition_EmptyInput()
    {
        string input = ""; // Пустая строка
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NonMatchingInput()
    {
        string input = "Это устройство не связано с месяцами.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_PartialMonthMatch()
    {
        string input = "Скоро январь!";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.True(result.IsRecognized);
        Assert.Equal("1", result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "В январе и феврале проходят праздники.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.True(result.IsRecognized);
        Assert.Equal("1", result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NoRelatedContext()
    {
        string input = "Это просто текст, не связанный с месяцами.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }
}
