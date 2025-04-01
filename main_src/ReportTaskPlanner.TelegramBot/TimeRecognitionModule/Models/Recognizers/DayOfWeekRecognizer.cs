using System.Diagnostics;
using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

public sealed class DayOfWeekRecognizer : BasicVectorRecognizer
{
    public DayOfWeekRecognizer()
        : base(0.5f)
    {
        AddText("понедельн", "понедельник");
        AddText("понедельни", "понедельник");
        AddText("понедельник", "понедельник");
        AddText("вторник", "вторник");
        AddText("среда", "среда");
        AddText("среду", "среда");
        AddText("четверг", "четверг");
        AddText("пятниц", "пятница");
        AddText("пятница", "пятница");
        AddText("пятницу", "пятница");
        AddText("суббот", "суббота");
        AddText("суббота", "суббота");
        AddText("субботу", "суббота");
        AddText("воскресень", "воскресенье");
        AddText("воскресенье", "воскресенье");
    }

    public override async Task<TimeRecognition> TryRecognize(string input)
    {
        IVectorTextResult<string, string> result = await PerformRecognition(input);
        Option<IVectorTextResultItem<string, string>> mostAccurate = GetMostAccurateRecognition(
            result
        );
        return mostAccurate.HasValue switch
        {
            true => FromMetadata(mostAccurate.Value.Metadata!),
            false => new UnrecognizedTime(),
        };
    }

    private static DayOfWeekRecognition FromMetadata(string metadata) =>
        metadata switch
        {
            "понедельник" => DayOfWeekRecognition.Monday,
            "вторник" => DayOfWeekRecognition.Tuesday,
            "среда" => DayOfWeekRecognition.Wednesday,
            "четверг" => DayOfWeekRecognition.Thursday,
            "пятница" => DayOfWeekRecognition.Friday,
            "суббота" => DayOfWeekRecognition.Saturday,
            "воскресенье" => DayOfWeekRecognition.Sunday,
            _ => throw new UnreachableException(),
        };
}
