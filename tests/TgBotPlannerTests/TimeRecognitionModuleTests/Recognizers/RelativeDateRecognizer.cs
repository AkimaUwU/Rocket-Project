using System.Diagnostics;
using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

public sealed class RelativeDateRecognizer : BasicRecognizer
{
    public RelativeDateRecognizer()
        : base(0.6f)
    {
        AddText("сегодня", "today");
        AddText("завтра", "yesterday");
        AddText("послезавтра", "dayaftertomorrow");
        AddText("после завтра", "dayaftertomorrow");
    }

    public override async Task<TimeRecognition> TryRecognize(string input)
    {
        IVectorTextResult<string, string> result = await PerformRecognition(input);
        Option<IVectorTextResultItem<string, string>> mostAccurate = GetMostAccurateRecognition(
            result
        );
        return mostAccurate.HasValue switch
        {
            true => MatchFromMetadata(mostAccurate.Value.Metadata!),
            false => new UnrecognizedTime(),
        };
    }

    private static RelativeRecognition MatchFromMetadata(string metaData) =>
        metaData switch
        {
            "today" => new RelativeRecognition(0),
            "yesterday" => new RelativeRecognition(1),
            "dayaftertomorrow" => new RelativeRecognition(2),
            _ => throw new UnreachableException(),
        };
}
