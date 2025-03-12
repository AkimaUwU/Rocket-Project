using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.RegexDateTimeParser.Types.DateTypes;

public abstract record DateType;

public sealed record UnknownDateType : DateType;

public sealed record AbsoluteDateType(DateTime Date) : DateType;

public sealed record RelativeDateType(DateTime Date) : DateType;

public sealed record RelativeWithTimeDateType(DateTime Date) : DateType;

public sealed record WeekedWithTimeDateType(DateTime Date) : DateType;

public sealed record MonthDateType(DateTime Date) : DateType;

public static class DateTypeExtensions
{
    public static Result<DateTime> GetDateTime(this DateType dateType) =>
        dateType switch
        {
            UnknownDateType => new Error("Не удалось определить дату"),
            AbsoluteDateType d => d.Date,
            RelativeDateType r => r.Date,
            RelativeWithTimeDateType wr => wr.Date,
            WeekedWithTimeDateType ww => ww.Date,
            MonthDateType m => m.Date,
            _ => new Error("Не удалось определить дату"),
        };
}
