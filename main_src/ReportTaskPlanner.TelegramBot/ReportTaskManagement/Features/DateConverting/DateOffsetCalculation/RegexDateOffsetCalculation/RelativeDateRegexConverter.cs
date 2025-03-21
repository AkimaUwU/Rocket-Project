using System.Diagnostics;
using System.Text.RegularExpressions;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;

public sealed partial class RelativeDateRegexConverter()
    : RegexDateOffsetCalculation.RegexDateOffsetCalculation(Regex())
{
    public override Option<DateOffsetResult> Convert(string stringDate, ApplicationTime time)
    {
        Match match = _regex.Match(stringDate);
        if (!match.Success)
            return Option<DateOffsetResult>.None();
        DateTime resultDate = GetDateTimeOffsetFromMatch(match, time);
        DateOffsetResult result = new DateOffsetResult(resultDate);
        return Option<DateOffsetResult>.Some(result);
    }

    private static DateTime GetDateTimeOffsetFromMatch(Match match, ApplicationTime time)
    {
        DateTime currentClone = new(time.DateTime.Year, time.DateTime.Month, time.DateTime.Day);
        string matchedString = match.Groups[1].Value;
        return matchedString.ToLower() switch
        {
            "сегодня" => currentClone,
            "завтра" => currentClone.AddDays(1),
            "послезавтра" => currentClone.AddDays(2),
            "после завтра" => currentClone.AddDays(2),
            _ => throw new UnreachableException(),
        };
    }

    [GeneratedRegex(
        @"(сегодня|завтра|послезавтра|после завтра)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
    )]
    private static partial Regex Regex();
}
