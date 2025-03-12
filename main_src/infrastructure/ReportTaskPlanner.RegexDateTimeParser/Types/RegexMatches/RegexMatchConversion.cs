using ReportTaskPlanner.RegexDateTimeParser.Types.DateTypes;
using ReportTaskPlanner.RegexDateTimeParser.Types.DayOfWeekType;
using ReportTaskPlanner.RegexDateTimeParser.Types.MonthTypes;

namespace ReportTaskPlanner.RegexDateTimeParser.Types.RegexMatches;

public static class RegexMatchConversion
{
    public static DateType ConvertToDateType(this RegexMatch match)
    {
        return match switch
        {
            UnknownRegexMatch => new UnknownDateType(),
            RelativeWithTimeRegexMatch r => r.FromRelativeWithTime(),
            AbsoluteRegexMatch a => a.FromAbsoluteMatch(),
            DayOfWeekRegexMatch w => w.FromDayOfWeekDate(),
            MonthWithTimeRegexMatch m => m.FromMonthWithTimeDate(),
            _ => new UnknownDateType(),
        };
    }

    private static DateType FromRelativeWithTime(this RelativeWithTimeRegexMatch match)
    {
        RelativeRegexMatch relative = new RelativeRegexMatch(match.MatchedInput);
        DateType dateType = relative.FromRelativeMatch();
        if (dateType is RelativeDateType relativeDateType)
        {
            DateTime date = relativeDateType.Date.Date;
            date = date.AddHours(match.Hours).AddMinutes(match.Minutes);
            return new RelativeWithTimeDateType(date);
        }
        return new UnknownDateType();
    }

    private static DateType FromRelativeMatch(this RelativeRegexMatch match) =>
        match.MatchedInput switch
        {
            null => new UnknownDateType(),
            not null when string.IsNullOrWhiteSpace(match.MatchedInput) => new UnknownDateType(),
            not null
                when match.MatchedInput.StartsWith("сегодн", StringComparison.OrdinalIgnoreCase) =>
                new RelativeDateType(DateTime.Now.Date),
            not null
                when match.MatchedInput.StartsWith("завтр", StringComparison.OrdinalIgnoreCase) =>
                new RelativeDateType(DateTime.Now.Date.AddDays(+1)),
            not null
                when match.MatchedInput.StartsWith(
                    "послезавтр",
                    StringComparison.OrdinalIgnoreCase
                ) => new RelativeDateType(DateTime.Now.Date.AddDays(+1)),
            not null
                when match.MatchedInput.StartsWith(
                    "после завтр",
                    StringComparison.OrdinalIgnoreCase
                ) => new RelativeDateType(DateTime.Now.Date.AddDays(+1)),
            _ => throw new ArgumentException("Invalid relative match"),
        };

    private static DateType FromAbsoluteMatch(this AbsoluteRegexMatch match)
    {
        int day = match.Day;
        int month = match.Month;
        int year = match.Year;
        DateTime date = new DateTime(year, month, day);
        return new AbsoluteDateType(date);
    }

    private static DateType FromDayOfWeekDate(this DayOfWeekRegexMatch match)
    {
        DayOfWeekType.DayOfWeekType dayOfWeek = match.DayOfWeek;
        if (dayOfWeek is UnknownDayOfWeekType)
            return new UnknownDateType();

        DayOfWeekType.DayOfWeekType currentDayOfWeek = DayOfWeekTypeGenerator.GenerateFromCurrent();
        int currentDayOfWeekNumber = currentDayOfWeek.Number;

        int hours = match.Hours;
        int minutes = match.Minutes;
        int dayOfWeekNumber = dayOfWeek.Number;

        int difference = dayOfWeekNumber - currentDayOfWeekNumber; // if date is today, then returns relative date type with adjusted hours and minutes
        if (difference == 0)
            return new RelativeWithTimeRegexMatch("Сегодня", hours, minutes).FromRelativeWithTime();

        return (difference <= 0) switch
        {
            true => new WeekedWithTimeDateType(
                DateTime.Now.Date.AddDays(difference + 7).AddHours(hours).AddMinutes(minutes) // if day of week in the past, then adjust to the next week
            ),
            false => new WeekedWithTimeDateType(
                DateTime.Now.Date.AddDays(difference).AddHours(hours).AddMinutes(minutes) // if day of week in the future, then just add days
            ),
        };
    }

    private static DateType FromMonthWithTimeDate(this MonthWithTimeRegexMatch match)
    {
        if (match.Month is UnknownMonthType)
            return new UnknownDateType();

        int monthNumber = match.Month.GetMonthNumber();
        int hours = match.Hours;
        int minutes = match.Minutes;
        int monthDay = match.MonthNumber;

        int currentMonth = DateTime.Now.Month;

        int year = (monthNumber < currentMonth) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

        if (monthDay > DateTime.DaysInMonth(year, monthNumber)) // if day of month is not a part of month. Then unknown date type.
            return new UnknownDateType();

        DateTime date = new DateTime(year, monthNumber, monthDay)
            .AddHours(hours)
            .AddMinutes(minutes);
        MonthDateType dateType = new MonthDateType(date);
        return dateType;
    }
}
