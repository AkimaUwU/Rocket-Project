using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace TgBotPlannerTests.Models;

public sealed class PeriodicTimeRecognizer : BasicRecognizer
{
    public PeriodicTimeRecognizer()
        : base(0.9f)
    {
        AddText("каждый понедельник", "every");
        AddText("каждый вторник", "every");
        AddText("каждый четверг", "every");
        AddText("каждую пятницу", "every");
        AddText("каждую субботу", "every");
        AddText("каждую среду", "every");
        AddText("каждое воскресенье", "every");
        AddText("каждый день", "every");
    }

    public override async Task<RecognitionResult> TryRecognize(string input)
    {
        string formatted = input.ToLowerInvariant().CleanString();
        IVectorTextResult<string, string> result = await Database.SearchAsync(
            formatted,
            threshold: Threshold
        );
        if (!result.Texts.Any())
            return new RecognitionResult(false, string.Empty);
        IVectorTextResultItem<string, string> item = result.Texts.First();
        return new RecognitionResult(true, item.Metadata!);
    }
}
