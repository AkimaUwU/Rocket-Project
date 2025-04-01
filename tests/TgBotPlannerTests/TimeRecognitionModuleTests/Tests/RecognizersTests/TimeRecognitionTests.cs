using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;
using TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.RecognizersTests;

public class TimeRecognitionTests
{
    private readonly RecognitionProcessor _processor;

    public TimeRecognitionTests()
    {
        IRecognitionStrategy strategy = new RawStringRecognitionStrategy();
        TimeRecognizer timeRecognizer = new();
        _processor = new RecognitionProcessor { Strategy = strategy };
        _processor.AddRecognizer(timeRecognizer);
    }

    [Fact]
    public async Task Invoke()
    {
        string input1 = "Сегодня в 23 59 сделать коммит в гитхаб";
        string input2 = "Сегодня в 22 41 сделать коммит в гитхаб";
        string input3 = "Сегодня в 122 441 сделать коммит в гитхаб";
        string input4 = "Сегодня в 008 035 сделать коммит в гитхаб";
        TimeRecognition result1 = await _processor.PerformRecognition(input1);
        TimeRecognition result2 = await _processor.PerformRecognition(input2);
        TimeRecognition result3 = await _processor.PerformRecognition(input3);
        TimeRecognition result4 = await _processor.PerformRecognition(input4);
        Assert.True(RecognitionValidator.IsRecognized(result1));
        Assert.True(RecognitionValidator.IsRecognized(result2));
        Assert.False(RecognitionValidator.IsRecognized(result3));
        Assert.False(RecognitionValidator.IsRecognized(result4));
    }
}
