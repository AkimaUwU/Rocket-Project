using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models.Recognizers;

public abstract class BasicRecognizer : ITimeRecognizer
{
    protected readonly BasicMemoryVectorDatabase Database = new();
    protected readonly float Threshold;

    protected BasicRecognizer(float threshold)
    {
        Threshold = threshold;
    }

    public abstract Task<TimeRecognition> TryRecognize(string input);

    protected void AddText(string text, string metadata) => Database.AddText(text, metadata);

    protected async Task<IVectorTextResult<string, string>> PerformRecognition(string input) =>
        await Database.SearchAsync(input, threshold: Threshold);

    protected static Option<IVectorTextResultItem<string, string>> GetMostAccurateRecognition(
        IVectorTextResult<string, string> result
    )
    {
        if (!result.Texts.Any())
            return Option<IVectorTextResultItem<string, string>>.None();
        IVectorTextResultItem<string, string> mostAccurate = result.Texts.First();
        return Option<IVectorTextResultItem<string, string>>.Some(mostAccurate);
    }
}
