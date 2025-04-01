using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;
using TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.RecognizersTests;

public class PeriodicTimeTypeTests
{
    private readonly RecognitionProcessor _processor;

    public PeriodicTimeTypeTests()
    {
        PeriodicTimeRecognizer periodic = new();
        IRecognitionStrategy strategy = new RawStringRecognitionStrategy();
        _processor = new RecognitionProcessor { Strategy = strategy };
        _processor.AddRecognizer(periodic);
    }

    [Fact]
    public async Task Test_Recognition_WithDifferentVariations()
    {
        string input1 = "Каждый день я занимаюсь спортом";
        string input2 = "Каждые выходные мы едем на дачу";
        string input3 = "С каждой недельной встречей все становится лучше";
        string input4 = "Каждый вторник я встречаюсь с друзьями";
        TimeRecognition result1 = await _processor.PerformRecognition(input1);
        TimeRecognition result2 = await _processor.PerformRecognition(input2);
        TimeRecognition result3 = await _processor.PerformRecognition(input3);
        TimeRecognition result4 = await _processor.PerformRecognition(input4);
        Assert.True(RecognitionValidator.IsRecognized(result1));
        Assert.False(RecognitionValidator.IsRecognized(result2));
        Assert.False(RecognitionValidator.IsRecognized(result3));
        Assert.True(RecognitionValidator.IsRecognized(result4));
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
        string input = "Этот текст не содержит никаких ключевых слов.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "Каждый день и каждые выходные я пью кофе.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.True(RecognitionValidator.IsRecognized(result));
    }

    [Fact]
    public async Task Test_Recognition_NoTimeContext()
    {
        string input = "Каждую чашку кофе.";
        TimeRecognition result = await _processor.PerformRecognition(input);
        Assert.False(RecognitionValidator.IsRecognized(result));
    }
}
