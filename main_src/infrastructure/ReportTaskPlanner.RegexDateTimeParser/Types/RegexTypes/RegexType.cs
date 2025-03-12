using System.Text.RegularExpressions;

namespace ReportTaskPlanner.RegexDateTimeParser.Types.RegexTypes;

public static class DefaultRegexOptions
{
    public static readonly RegexOptions Options =
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;
}

public abstract record RegexType(Regex Regex);

public sealed record RelativeRegexType()
    : RegexType(
        new Regex(@"\b(сегодня|завтра|послезавтра|после завтра)\b", DefaultRegexOptions.Options)
    );

public sealed record RelativeWithTimeRegexType()
    : RegexType(
        new Regex(
            @"\b(сегодня|завтра|послезавтра|после завтра)\b.*?(\d{1,2})\s*[:\s]?\s*(\d{2})?",
            DefaultRegexOptions.Options
        )
    );

public sealed record TimeRegexType()
    : RegexType(new Regex(@"\b(\d{1,2})[:\s](\d{2})\b", DefaultRegexOptions.Options));

public sealed record AbsoluteRegexType()
    : RegexType(
        new Regex(
            @"\b(\d{1,2}\.\d{1,2}\.\d{2,4}|\d{1,2}[\s./-]\d{1,2}[\s./-]\d{2,4})\b",
            DefaultRegexOptions.Options
        )
    );

public sealed record DayOfWeekRegexType()
    : RegexType(
        new Regex(
            @"(?:\bв[о]?\s+)?\b(понедельн|вторн|сред|четверг?|пятниц|суббот|воскресен)\w*\b.*?(\d{1,2})\s*[:\s]?\s*(\d{2})",
            DefaultRegexOptions.Options
        )
    );

public sealed record MonthWithNumberAndTimeRegexType()
    : RegexType(
        new Regex(
            @"((?:\d{1,2}\s+)?(?:январ[яею]?|феврал[яею]?|март[аеу]?|апрел[яею]?|ма[яею]?|июн[яею]?|июл[яею]?|авг[устаею]?|сентябр[яею]?|октябр[яею]?|ноябр[яею]?|декабр[яею]?)\s*(?:\d{1,2})?)\b(?:\D+((?:\d{1,2})[:\s](?:\d{2})))?",
            DefaultRegexOptions.Options
        )
    );

public static class RegexTypeList
{
    public static RegexType[] Array =>
        [
            new MonthWithNumberAndTimeRegexType(),
            new DayOfWeekRegexType(),
            new RelativeWithTimeRegexType(),
            new RelativeRegexType(),
            new AbsoluteRegexType(),
        ];
}
