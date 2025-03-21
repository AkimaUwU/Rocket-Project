using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.Months;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;

public sealed partial class MonthDateRegexConverter()
    : RegexDateOffsetCalculation.RegexDateOffsetCalculation(MonthRegex())
{
    public override Option<DateOffsetResult> Convert(string stringDate, ApplicationTime time)
    {
        Match match = _regex.Match(stringDate);
        if (!match.Success)
            return Option<DateOffsetResult>.None();
        if (match.Groups.Count != 3)
            return Option<DateOffsetResult>.None();

        Option<int> parsedMonthNumber = GetMonthNumberFromMatch(match);
        if (!parsedMonthNumber.HasValue)
            return Option<DateOffsetResult>.None();

        int monthNumber = parsedMonthNumber.Value;
        int parsedDayOfMonth = GetDayOfMonthFromMatch(match);
        Option<DateTime> resultDate = GetDateWithMonthOffset(
            ref monthNumber,
            ref parsedDayOfMonth,
            time
        );
        if (!resultDate.HasValue)
            return Option<DateOffsetResult>.None();

        DateOffsetResult result = new(resultDate);
        return Option<DateOffsetResult>.Some(result);
    }

    private static Option<DateTime> GetDateWithMonthOffset(
        ref int monthNumber,
        ref int monthDay,
        ApplicationTime time
    )
    {
        try
        {
            DateTime currentDate = time.DateTime;
            int year = currentDate.Year;
            DateTime newDate = new(year, monthNumber, monthDay);
            if (newDate < currentDate)
                newDate = newDate.AddYears(1);
            return Option<DateTime>.Some(newDate);
        }
        catch
        {
            return Option<DateTime>.None();
        }
    }

    private static Option<int> GetMonthNumberFromMatch(Match match)
    {
        string matchedMonthName = match.Groups[2].Value;
        Option<MonthType> monthConvert = MonthGenerator.TryGenerateFromInput(matchedMonthName);
        if (!monthConvert.HasValue)
            return Option<int>.None();
        int monthNumber = monthConvert.Value.GetMonthNumber();
        return Option<int>.Some(monthNumber);
    }

    private static int GetDayOfMonthFromMatch(Match match)
    {
        string matchedDayOfMonth = match.Groups[1].Value;
        return int.Parse(matchedDayOfMonth);
    }

    [GeneratedRegex(
        @"(\d+)\s+(январ[яею]?|феврал[яею]?|март[аеу]?|апрел[яею]?|ма[яею]?|июн[яею]?|июл[яею]?|авг[устаею]?|сентябр[яею]?|октябр[яею]?|ноябр[яею]?|декабр[яею]?)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
    )]
    private static partial Regex MonthRegex();
}
