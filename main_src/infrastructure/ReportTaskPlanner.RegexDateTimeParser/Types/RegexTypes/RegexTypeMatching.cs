using System.Text.RegularExpressions;
using ReportTaskPlanner.RegexDateTimeParser.Types.DayOfWeekType;
using ReportTaskPlanner.RegexDateTimeParser.Types.MonthTypes;
using ReportTaskPlanner.RegexDateTimeParser.Types.RegexMatches;

namespace ReportTaskPlanner.RegexDateTimeParser.Types.RegexTypes;

public static class RegexTypeMatching
{
    public static RegexMatch TryMatch(string input, params RegexType[] regexes)
    {
        foreach (var regex in regexes)
        {
            RegexMatch match = regex.Match(input);
            if (match is UnknownRegexMatch)
                continue;
            return match;
        }
        return new UnknownRegexMatch();
    }

    private static RegexMatch Match(this RegexType regexType, string input)
    {
        return regexType switch
        {
            MonthWithNumberAndTimeRegexType m => m.FromMonth(input),
            RelativeWithTimeRegexType r => r.FromRelativeWithTime(input),
            DayOfWeekRegexType w => w.FromDayOfWeek(input),
            RelativeRegexType r => r.FromRelative(input),
            AbsoluteRegexType a => a.FromAbsolute(input),
            _ => throw new ArgumentException("Unknown regex type"),
        };
    }

    private static RegexMatch FromRelative(this RelativeRegexType relative, string input)
    {
        Regex regex = relative.Regex;
        Match match = regex.Match(input);
        if (!match.Success)
            return new UnknownRegexMatch();
        string matchedString = match.Groups[1].Value;
        return new RelativeRegexMatch(matchedString);
    }

    private static RegexMatch FromAbsolute(this AbsoluteRegexType absolute, string input)
    {
        Regex regex = absolute.Regex;
        Match match = regex.Match(input);
        if (!match.Success)
            return new UnknownRegexMatch();
        string matchedDay = match.Groups[1].Value;
        string matchedMonth = match.Groups[2].Value;
        string matchedYear = match.Groups[3].Value;
        return new AbsoluteRegexMatch(
            int.Parse(matchedDay),
            int.Parse(matchedMonth),
            int.Parse(matchedYear)
        );
    }

    private static RegexMatch FromRelativeWithTime(
        this RelativeWithTimeRegexType relativeWithTime,
        string input
    )
    {
        Regex regex = relativeWithTime.Regex;
        Match match = regex.Match(input);
        if (!match.Success)
            return new UnknownRegexMatch();
        string matchedString = match.Groups[1].Value;
        string hoursString = match.Groups[2].Value;
        string minutesString = match.Groups[3].Value;
        int hours = int.Parse(hoursString);
        int minutes = int.Parse(minutesString);
        return new RelativeWithTimeRegexMatch(matchedString, hours, minutes);
    }

    private static RegexMatch FromDayOfWeek(this DayOfWeekRegexType dayOfWeekRegex, string input)
    {
        Regex regex = dayOfWeekRegex.Regex;
        Match match = regex.Match(input);
        if (!match.Success)
            return new UnknownRegexMatch();
        string dayOfWeekString = match.Groups[1].Value;
        string hoursString = match.Groups[2].Value;
        string minutesString = match.Groups[3].Value;
        DayOfWeekType.DayOfWeekType dayOfWeek = DayOfWeekTypeGenerator.GenerateFromInput(
            dayOfWeekString
        );
        if (dayOfWeek is not UnknownDayOfWeekType)
        {
            int hours = int.Parse(hoursString);
            int minutes = int.Parse(minutesString);
            return new DayOfWeekRegexMatch(dayOfWeek, hours, minutes);
        }
        return new UnknownRegexMatch();
    }

    private static RegexMatch FromMonth(
        this MonthWithNumberAndTimeRegexType monthRegex,
        string input
    )
    {
        Regex regex = monthRegex.Regex;
        Match match = regex.Match(input);
        if (!match.Success)
            return new UnknownRegexMatch();

        string monthWithNumberGroup = match.Groups[1].Value.Trim();
        string timeGroup = match.Groups[2].Value.Trim();

        if (string.IsNullOrWhiteSpace(monthWithNumberGroup))
            return new UnknownRegexMatch();
        if (string.IsNullOrWhiteSpace(timeGroup))
            return new UnknownRegexMatch();

        string[] splittedMonthData = monthWithNumberGroup.Split(' ');
        if (splittedMonthData.Length != 2)
            return new UnknownRegexMatch();

        TimeRegexType time = new TimeRegexType();
        Match timeMatch = time.Regex.Match(timeGroup);
        if (!timeMatch.Success)
            return new UnknownRegexMatch();

        string monthNumberString = splittedMonthData[0];
        string monthString = splittedMonthData[1];

        string hoursString = timeMatch.Groups[1].Value;
        string minutesString = timeMatch.Groups[2].Value;

        int monthNumber = int.Parse(monthNumberString);
        int hours = int.Parse(hoursString);
        int minutes = int.Parse(minutesString);

        MonthType month = MonthType.Months.GetMatchedMonthType(monthString);
        if (month is UnknownMonthType)
            return new UnknownRegexMatch();

        return new MonthWithTimeRegexMatch(month, monthNumber, hours, minutes);
    }
}
