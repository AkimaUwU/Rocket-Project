using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace TgBotPlannerTests.Models;

public sealed class TimeRecognizer : BasicRecognizer
{
    public TimeRecognizer()
        : base(1.0f)
    {
        for (int hours = 0; hours <= 23; hours++)
        {
            string hoursString = CreateTimeString(hours);
            for (int minutes = 0; minutes <= 59; minutes++)
            {
                string minutesString = CreateTimeString(minutes);
                string timeString = CombineHoursAndMinutes(hoursString, minutesString);
                AddText(timeString, timeString);
            }
        }
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

    private static string CreateTimeString(int unit) => unit.ToString("D2");

    private static string CombineHoursAndMinutes(string hoursString, string minutesString) =>
        $"{hoursString} {minutesString}";
}
