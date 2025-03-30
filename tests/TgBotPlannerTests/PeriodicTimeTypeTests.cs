using TgBotPlannerTests.Models;

namespace TgBotPlannerTests;

public class PeriodicTimeTypeTests
{
    private readonly PeriodicTimeRecognizer recognizer = new();

    [Fact]
    public async Task Test_Recognition_WithDifferentVariations()
    {
        string input1 = "Каждый день я занимаюсь спортом";
        string input2 = "Каждые выходные мы едем на дачу";
        string input3 = "С каждой недельной встречей все становится лучше";
        string input4 = "Каждый вторник я встречаюсь с друзьями";
        RecognitionResult result1 = await recognizer.TryRecognize(input1);
        RecognitionResult result2 = await recognizer.TryRecognize(input2);
        RecognitionResult result3 = await recognizer.TryRecognize(input3);
        RecognitionResult result4 = await recognizer.TryRecognize(input4);
        Assert.True(result1.IsRecognized);
        Assert.Equal("every", result1.RecognizedMetadata);
        Assert.False(result2.IsRecognized);
        Assert.False(result3.IsRecognized);
        Assert.True(result4.IsRecognized);
        Assert.Equal("every", result4.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_EmptyInput()
    {
        string input = "";
        RecognitionResult result = await recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NonMatchingInput()
    {
        string input = "Этот текст не содержит никаких ключевых слов.";
        RecognitionResult result = await recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
        Assert.Equal(string.Empty, result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_MultipleOccurrences()
    {
        string input = "Каждый день и каждые выходные я пью кофе.";
        RecognitionResult result = await recognizer.TryRecognize(input);
        Assert.True(result.IsRecognized);
        Assert.Equal("every", result.RecognizedMetadata);
    }

    [Fact]
    public async Task Test_Recognition_NoTimeContext()
    {
        string input = "Каждую чашку кофе.";
        RecognitionResult result = await recognizer.TryRecognize(input);
        Assert.False(result.IsRecognized);
    }
}
