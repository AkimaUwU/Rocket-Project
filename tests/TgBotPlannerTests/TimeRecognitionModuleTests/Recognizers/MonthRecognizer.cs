using System.Diagnostics;
using System.Text.RegularExpressions;
using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Recognizers;

public sealed partial class MonthRecognizer : BasicRecognizer
{
    public MonthRecognizer()
        : base(0.7f)
    {
        AddText("январ", "1");
        AddText("январь", "1");
        AddText("января", "1");
        AddText("январе", "1");
        AddText("феврал", "2");
        AddText("февраль", "2");
        AddText("февраля", "2");
        AddText("феврале", "2");
        AddText("март", "3");
        AddText("марта", "3");
        AddText("марте", "3");
        AddText("апрел", "4");
        AddText("апрель", "4");
        AddText("апреля", "4");
        AddText("апреле", "4");
        AddText("май", "5");
        AddText("мая", "5");
        AddText("маю", "5");
        AddText("мае", "5");
        AddText("июн", "6");
        AddText("июнь", "6");
        AddText("июня", "6");
        AddText("июню", "6");
        AddText("июне", "6");
        AddText("июл", "7");
        AddText("июль", "7");
        AddText("июля", "7");
        AddText("июлю", "7");
        AddText("июле", "7");
        AddText("август", "8");
        AddText("августа", "8");
        AddText("августу", "8");
        AddText("августе", "8");
        AddText("сентябр", "9");
        AddText("сентябрь", "9");
        AddText("сентября", "9");
        AddText("сентябрю", "9");
        AddText("сентябре", "9");
        AddText("октябр", "10");
        AddText("октябрь", "10");
        AddText("октября", "10");
        AddText("октябрю", "10");
        AddText("октябре", "10");
        AddText("ноябр", "11");
        AddText("ноябрь", "11");
        AddText("ноября", "11");
        AddText("ноябрю", "11");
        AddText("ноябре", "11");
        AddText("декабр", "12");
        AddText("декабрь", "12");
        AddText("декабря", "12");
        AddText("декабрю", "12");
        AddText("декабре", "12");
    }

    public override async Task<TimeRecognition> TryRecognize(string input)
    {
        Option<int> day = GetDayBeforeMonth(input);
        if (!day.HasValue)
            return new UnrecognizedTime();
        int dayValue = day.Value;
        IVectorTextResult<string, string> result = await PerformRecognition(input);
        Option<IVectorTextResultItem<string, string>> mostAccurate = GetMostAccurateRecognition(
            result
        );
        return mostAccurate.HasValue switch
        {
            true => FromMetadata(mostAccurate.Value.Metadata!, dayValue),
            false => new UnrecognizedTime(),
        };
    }

    private static MonthRecognition FromMetadata(string metadata, int dayValue) =>
        metadata switch
        {
            "1" => new MonthRecognition(1, dayValue),
            "2" => new MonthRecognition(2, dayValue),
            "3" => new MonthRecognition(3, dayValue),
            "4" => new MonthRecognition(4, dayValue),
            "5" => new MonthRecognition(5, dayValue),
            "6" => new MonthRecognition(6, dayValue),
            "7" => new MonthRecognition(7, dayValue),
            "8" => new MonthRecognition(8, dayValue),
            "9" => new MonthRecognition(9, dayValue),
            "10" => new MonthRecognition(10, dayValue),
            "11" => new MonthRecognition(11, dayValue),
            "12" => new MonthRecognition(12, dayValue),
            _ => throw new UnreachableException(),
        };

    [GeneratedRegex(
        @"(\d{1,2})\s*(январ(я|ь|е)?|феврал(я|ь|е)?|мар(та|т)|апрел(я|ь)?|ма(й|я)?|июн(я|ь|е)?|июл(я|ь|е)?|август(а|е)?|сентябр(я|ь|е)?|октябр(я|ь|е)?|ноябр(я|ь|е)?|декабр(я|ь|е)?)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
    )]
    private static partial Regex DayBeforeMonthRegex();

    private static Option<int> GetDayBeforeMonth(string input)
    {
        Match match = DayBeforeMonthRegex().Match(input);
        if (!match.Success || match.Length < 2)
            return Option<int>.None();
        string dayString = match.Groups[1].Value;
        return string.IsNullOrWhiteSpace(dayString)
            ? Option<int>.None()
            : Option<int>.Some(int.Parse(dayString));
    }
}
