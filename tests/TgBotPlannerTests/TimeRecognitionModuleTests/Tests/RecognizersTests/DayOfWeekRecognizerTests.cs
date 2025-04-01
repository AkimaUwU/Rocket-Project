using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;
using TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.RecognizersTests;

public class DayOfWeekRecognizerTests
{
    private readonly RecognitionProcessor _processor;

    public DayOfWeekRecognizerTests()
    {
        DayOfWeekRecognizer recognizer = new();
        IRecognitionStrategy strategy = new ChunkRecognitionStrategy();
        _processor = new RecognitionProcessor() { Strategy = strategy };
        _processor.AddRecognizer(recognizer);
    }

    [Fact]
    public async Task Test_Recognition_ValidDays()
    {
        string[] inputs =
        [
            "понедельн",
            "вторник",
            "среда",
            "среду",
            "четверг",
            "пятниц",
            "суббот",
            "воскресень",
        ];

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
        string input = "В этом предложении нет дней недели.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_PartialDayMatch()
    {
        string input = "Скоро понедельник!";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "Каждый понедельник и среду я иду в спортзал.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_NoRelatedContext()
    {
        string input = "Это просто текст, не связанный с днями недели.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }
}
