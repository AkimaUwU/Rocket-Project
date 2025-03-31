using TgBotPlannerTests.Models.Recognitions;
using TgBotPlannerTests.Models.RecognitionStrategies;
using TgBotPlannerTests.Models.Recognizers;

namespace TgBotPlannerTests.Models.Tests.RecognizersTests;

public class MonthRecognitionTests
{
    private readonly RecognitionProcessor _processor;

    public MonthRecognitionTests()
    {
        MonthRecognizer monthRecognizer = new();
        IRecognitionStrategy strategy = new RawStringRecognitionStrategy();
        _processor = new RecognitionProcessor() { Strategy = strategy };
        _processor.AddRecognizer(monthRecognizer);
    }

    [Fact]
    public async Task Test_Recognition_ValidMonths()
    {
        string[] inputs =
        [
            "14 январ",
            "23 феврал",
            "15 март",
            "16 апрел",
            "17 май",
            "8 мая",
            "12 маю",
            "13 июн",
            "4 июл",
            "2 август",
            "1 сентябр",
            "3 октябр",
            "55 ноябр",
            "23 декабр",
        ];

        foreach (string t in inputs)
        {
            TimeRecognition result = await _processor.PerformRecognition(t);
            Assert.True(RecognitionValidator.IsRecognized(result)); // because partial used.
        }
    }

    [Fact]
    public async Task Test_Recognition_EmptyInput()
    {
        string input = "";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_NonMatchingInput()
    {
        string input = "Это устройство не связано с месяцами.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_PartialMonthMatch()
    {
        string input = "Скоро январь!";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "В январе и феврале проходят праздники.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_NoRelatedContext()
    {
        string input = "Это просто текст, не связанный с месяцами.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }
}
