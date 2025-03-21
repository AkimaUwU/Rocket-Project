using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DayOfWeeks;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;

public sealed partial class DayOfWeekRegexConverter()
    : RegexDateOffsetCalculation.RegexDateOffsetCalculation(MyRegex())
{
    public override Option<DateOffsetResult> Convert(string stringDate, ApplicationTime time)
    {
        Match match = _regex.Match(stringDate);
        if (!match.Success)
            return Option<DateOffsetResult>.None();

        Option<DayOfWeekType> dayOfWeek = GetDayOfWeekFromMatch(match);
        if (!dayOfWeek.HasValue)
            return Option<DateOffsetResult>.None();

        DateTime resultDate = CreateDateWithDayOfWeekOffset(dayOfWeek, time);
        DateOffsetResult result = new(resultDate);
        return Option<DateOffsetResult>.Some(result);
    }

    private static DateTime CreateDateWithDayOfWeekOffset(
        DayOfWeekType dayOfWeek,
        ApplicationTime time
    )
    {
        DateTime current = time.DateTime;
        int parsedDayOfWeek = dayOfWeek.GetDayNumber();
        int currentDayOfWeek = (int)current.DayOfWeek;
        int daysUntilRequested = parsedDayOfWeek - currentDayOfWeek;
        if (daysUntilRequested < 0)
            daysUntilRequested = daysUntilRequested += 7;
        return current.Date.AddDays(daysUntilRequested);
    }

    private static Option<DayOfWeekType> GetDayOfWeekFromMatch(Match match)
    {
        string matchedValue = match.Groups[1].Value;
        return DayOfWeekTypeGenerator.GenerateFromInput(matchedValue);
    }

    [GeneratedRegex(
        @"(\bпонедель\w*\b|\bвторн\w*\b|\bсред[у|а]\b|\bчетверг\w*\b|\bпятниц\w*\b|\bсуббот\w*\b|\bвоскресен\w*\b)"
    )]
    private static partial Regex MyRegex();
}
