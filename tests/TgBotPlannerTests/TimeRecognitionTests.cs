using TgBotPlannerTests.Models;

namespace TgBotPlannerTests;

public class TimeRecognitionTests
{
    private readonly TimeRecognizer _recognizer = new();

    [Fact]
    public async Task Invoke()
    {
        string input1 = "Сегодня в 23 59 сделать коммит в гитхаб";
        string input2 = "Сегодня в 22 41 сделать коммит в гитхаб";
        string input3 = "Сегодня в 122 441 сделать коммит в гитхаб";
        string input4 = "Сегодня в 008 035 сделать коммит в гитхаб";
        RecognitionResult result1 = await _recognizer.TryRecognize(input1);
        RecognitionResult result2 = await _recognizer.TryRecognize(input2);
        RecognitionResult result3 = await _recognizer.TryRecognize(input3);
        RecognitionResult result4 = await _recognizer.TryRecognize(input4);
        Assert.True(result1.IsRecognized);
        Assert.Equal("23 59", result1.RecognizedMetadata);
        Assert.True(result2.IsRecognized);
        Assert.Equal("22 41", result2.RecognizedMetadata);
        Assert.False(result3.IsRecognized);
        Assert.False(result4.IsRecognized);
    }
}
