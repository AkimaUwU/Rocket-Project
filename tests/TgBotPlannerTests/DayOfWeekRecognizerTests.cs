using TgBotPlannerTests.Models;

namespace TgBotPlannerTests;

public class DayOfWeekRecognizerTests
{
    private readonly DayOfWeekRecognizer _recognizer;

    public DayOfWeekRecognizerTests()
    {
        _recognizer = new DayOfWeekRecognizer();
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
        string[] expectedResults =
        [
            "понедельник",
            "вторник",
            "среда",
            "среда",
            "четверг",
            "пятница",
            "суббота",
            "воскресенье",
        ];

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
        string input = "";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NonMatchingInput()
    {
        string input = "В этом предложении нет дней недели.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_PartialDayMatch()
    {
        string input = "Скоро понедельник!";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.True(result.IsRecognized);
        Assert.Equal("понедельник", result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "Каждый понедельник и среду я иду в спортзал.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.True(result.IsRecognized);
        Assert.Equal("понедельник", result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NoRelatedContext()
    {
        string input = "Это просто текст, не связанный с днями недели.";
        RecognitionResult result = await _recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }
}
