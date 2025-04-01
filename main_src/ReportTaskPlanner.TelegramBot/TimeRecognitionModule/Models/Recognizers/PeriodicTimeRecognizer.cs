using System.Diagnostics;
using System.Text.RegularExpressions;
using Build5Nines.SharpVector;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

public sealed partial class PeriodicTimeRecognizer : BasicVectorRecognizer
{
    public PeriodicTimeRecognizer()
        : base(0.8f)
    {
        AddText("каждый понедельник", "every_monday");
        AddText("каждый вторник", "every_tuesday");
        AddText("каждый четверг", "every_thursday");
        AddText("каждую пятницу", "every_friday");
        AddText("каждую субботу", "every_saturday");
        AddText("каждую среду", "every_wednesday");
        AddText("каждое воскресенье", "every_sunday");
        AddText("каждый день", "every_day");
    }

    public override async Task<TimeRecognition> TryRecognize(string input)
    {
        IVectorTextResult<string, string> result = await PerformRecognition(input);
        Option<IVectorTextResultItem<string, string>> mostAccurate = GetMostAccurateRecognition(
            result
        );
        if (mostAccurate.HasValue)
            return FromMetadata(mostAccurate.Value.Metadata!);
        Option<PeriodicEveryDayRecognition> fromRegexEveryDay = EveryDayFromRegex(input);
        if (fromRegexEveryDay.HasValue)
            return fromRegexEveryDay.Value;
        Option<PeriodicEveryMinuteRecognition> fromRegexEveryMinute = EveryMinuteFromRegex(input);
        if (fromRegexEveryMinute.HasValue)
            return fromRegexEveryMinute.Value;
        Option<PeriodicEveryHourRecognition> fromRegexEveryHour = FromEveryHourRegex(input);
        if (fromRegexEveryHour.HasValue)
            return fromRegexEveryHour.Value;
        Option<PeriodicEveryHourRecognition> fromRegexEveryHourUnspecified =
            FromEveryHourUnspecifiedRegex(input);
        if (fromRegexEveryHourUnspecified.HasValue)
            return fromRegexEveryHourUnspecified.Value;
        Option<PeriodicEveryMinuteRecognition> fromRegexEveryMinuteUnspecified =
            FromEveryMinuteUnspecifiedRegex(input);
        if (fromRegexEveryMinuteUnspecified.HasValue)
            return fromRegexEveryMinuteUnspecified.Value;
        return new UnrecognizedTime();
    }

    private static Option<PeriodicEveryDayRecognition> EveryDayFromRegex(string input)
    {
        Option<string> matchedString = FromRegexMatch(EveryDayRegex(), input);
        return matchedString.HasValue
            ? Option<PeriodicEveryDayRecognition>.Some(FromEveryDayRecognition(matchedString.Value))
            : Option<PeriodicEveryDayRecognition>.None();
    }

    private static Option<PeriodicEveryMinuteRecognition> EveryMinuteFromRegex(string input)
    {
        Option<string> matchedString = FromRegexMatch(EveryMinuteRegex(), input);
        return matchedString.HasValue
            ? Option<PeriodicEveryMinuteRecognition>.Some(
                FromEveryMinuteRecognition(matchedString.Value)
            )
            : Option<PeriodicEveryMinuteRecognition>.None();
    }

    private static Option<PeriodicEveryHourRecognition> FromEveryHourRegex(string input)
    {
        Option<string> matchedString = FromRegexMatch(EveryHourRegex(), input);
        return matchedString.HasValue
            ? Option<PeriodicEveryHourRecognition>.Some(
                FromEveryHourRecognition(matchedString.Value)
            )
            : Option<PeriodicEveryHourRecognition>.None();
    }

    private static Option<PeriodicEveryHourRecognition> FromEveryHourUnspecifiedRegex(string input)
    {
        Match match = EveryHourNotSpecifiedRegex().Match(input);
        return !match.Success
            ? Option<PeriodicEveryHourRecognition>.None()
            : Option<PeriodicEveryHourRecognition>.Some(
                new PeriodicEveryHourRecognition(new SpecificTimeRecognition(1, 0))
            );
    }

    private static Option<PeriodicEveryMinuteRecognition> FromEveryMinuteUnspecifiedRegex(
        string input
    )
    {
        Match match = EveryMinuteNotSpecifiedRegex().Match(input);
        return !match.Success
            ? Option<PeriodicEveryMinuteRecognition>.None()
            : Option<PeriodicEveryMinuteRecognition>.Some(
                new PeriodicEveryMinuteRecognition(new SpecificTimeRecognition(0, 1))
            );
    }

    private static Option<string> FromRegexMatch(Regex regex, string input)
    {
        Match match = regex.Match(input);
        if (!match.Success || string.IsNullOrWhiteSpace(match.Groups[0].Value))
            return Option<string>.None();
        return Option<string>.Some(match.Groups[0].Value);
    }

    private static TimeRecognition FromMetadata(string metadata) =>
        metadata switch
        {
            "every_monday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Monday),
            "every_tuesday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Tuesday),
            "every_wednesday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Wednesday),
            "every_thursday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Thursday),
            "every_friday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Friday),
            "every_saturday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Saturday),
            "every_sunday" => new PeriodicWeekDayRecognition(DayOfWeekRecognition.Sunday),
            "every_day" => new PeriodicEveryDayRecognition(new RelativeRecognition(1)),
            _ => throw new UnreachableException(),
        };

    private static PeriodicEveryDayRecognition FromEveryDayRecognition(string text)
    {
        int offset = GetNumberOfOffset(text);
        return new PeriodicEveryDayRecognition(new RelativeRecognition(offset));
    }

    private static PeriodicEveryHourRecognition FromEveryHourRecognition(string text)
    {
        int offset = GetNumberOfOffset(text);
        return new PeriodicEveryHourRecognition(new SpecificTimeRecognition(offset, 0));
    }

    private static PeriodicEveryMinuteRecognition FromEveryMinuteRecognition(string text)
    {
        int offset = GetNumberOfOffset(text);
        return new PeriodicEveryMinuteRecognition(new SpecificTimeRecognition(0, offset));
    }

    private static int GetNumberOfOffset(string text)
    {
        ReadOnlySpan<char> onlyDigits = text.Where(char.IsDigit).ToArray();
        return onlyDigits.Length == 0 ? 1 : int.Parse(new string(onlyDigits));
    }

    private const RegexOptions Options =
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

    [GeneratedRegex(@"кажд(ый|ые|ую)\s+\d{1,2}\s*дн(я|ей)?", Options)]
    private static partial Regex EveryDayRegex();

    [GeneratedRegex(@"кажд(ый|ые|ую)\s+\d{1,2}\s*минут(у|ы)?", Options)]
    private static partial Regex EveryMinuteRegex();

    [GeneratedRegex(@"кажд(ый|ые|ую)\s+\d{1,2}\s*час(а|ов)?", Options)]
    private static partial Regex EveryHourRegex();

    [GeneratedRegex(@"(кажд(?:ый|ые|ую)\sча(?:с|а|ов))", Options)]
    private static partial Regex EveryHourNotSpecifiedRegex();

    [GeneratedRegex(@"кажд(?:ый|ые|ую)\sми(?:нуту|нут|н)", Options)]
    private static partial Regex EveryMinuteNotSpecifiedRegex();
}
