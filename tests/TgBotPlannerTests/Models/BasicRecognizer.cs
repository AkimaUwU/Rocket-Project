using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace TgBotPlannerTests.Models;

public abstract class BasicRecognizer
{
    protected readonly BasicMemoryVectorDatabase Database = new();
    protected readonly float Threshold;

    protected BasicRecognizer(float threshold)
    {
        Threshold = threshold;
    }

    public void AddText(string text, string metadata)
    {
        Database.AddText(text, metadata);
    }

    public virtual async Task<RecognitionResult> TryRecognize(string input)
    {
        string formatted = input.ToLowerInvariant().CleanString();
        string[] words = SplitIntoWords(formatted);
        foreach (string word in words)
        {
            RecognitionResult result = await Recognize(word, Threshold);
            if (result.IsRecognized)
                return result;
        }

        return new RecognitionResult(false, string.Empty);
    }

    private async Task<RecognitionResult> Recognize(string input, float threshold)
    {
        IVectorTextResult<string, string> result = await Database.SearchAsync(
            input,
            threshold: threshold
        );
        if (!result.Texts.Any())
            return new RecognitionResult(false, string.Empty);
        IVectorTextResultItem<string, string> item = result.Texts.First();
        return new RecognitionResult(true, item.Metadata!);
    }

    private static string[] SplitIntoWords(string input)
    {
        return input.Split(' ', StringSplitOptions.TrimEntries);
    }
}
