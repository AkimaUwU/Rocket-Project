using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;
using TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.RecognizersTests;

public class RelativeDateRecognizerTests
{
    private readonly RecognitionProcessor _processor;

    public RelativeDateRecognizerTests()
    {
        RelativeDateRecognizer relative = new();
        IRecognitionStrategy strategy = new ChunkRecognitionStrategy();
        _processor = new RecognitionProcessor { Strategy = strategy };
        _processor.AddRecognizer(relative);
    }

    [Fact]
    public async Task Test_Recognition_ValidRelativeDates()
    {
        string[] inputs = ["сегодня", "завтра", "послезавтра", "после завтра"];
        foreach (string t in inputs)
        {
            TimeRecognition result = await _processor.PerformRecognition(t);
            Assert.True(RecognitionValidator.IsRecognized(result));
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
        string input = "Сегодня хорошая погода.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_PartialDateMatch()
    {
        string input = "У нас план на сегодня!";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "Завтра и послезавтра будут встречи.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_NoRelatedContext()
    {
        string input = "Это просто текст без относительных дат.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }
}
