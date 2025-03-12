using ReportTaskPlanner.RegexDateTimeParser.Types.MonthTypes;

namespace ReportTaskPlanner.RegexDateTimeParser.Types.RegexMatches;

public abstract record RegexMatch;

public record RelativeRegexMatch(string MatchedInput) : RegexMatch;

public sealed record RelativeWithTimeRegexMatch(string MatchedInput, int Hours, int Minutes)
    : RegexMatch;

public sealed record AbsoluteRegexMatch(int Day, int Month, int Year) : RegexMatch;

public sealed record DayOfWeekRegexMatch(
    DayOfWeekType.DayOfWeekType DayOfWeek,
    int Hours,
    int Minutes
) : RegexMatch;

public sealed record MonthWithTimeRegexMatch(
    MonthType Month,
    int MonthNumber,
    int Hours,
    int Minutes
) : RegexMatch;

public sealed record UnknownRegexMatch : RegexMatch;
