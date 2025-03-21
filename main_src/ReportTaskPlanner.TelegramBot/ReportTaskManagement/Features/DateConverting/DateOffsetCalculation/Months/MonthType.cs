using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.Months;

public abstract record MonthType
{
    public abstract string StartsWithValue { get; }
    public abstract string DisplayName { get; }
    public abstract int MonthNumber { get; }
}

public sealed record January : MonthType
{
    public override string StartsWithValue { get; } = "январ";
    public override string DisplayName { get; } = "Январь";
    public override int MonthNumber { get; } = 1;
}

public sealed record February : MonthType
{
    public override string StartsWithValue { get; } = "феврал";
    public override string DisplayName { get; } = "Февраль";
    public override int MonthNumber { get; } = 2;
}

public sealed record March : MonthType
{
    public override string StartsWithValue { get; } = "март";
    public override string DisplayName { get; } = "Март";
    public override int MonthNumber { get; } = 3;
}

public sealed record April : MonthType
{
    public override string StartsWithValue { get; } = "апрел";
    public override string DisplayName { get; } = "Апрель";
    public override int MonthNumber { get; } = 4;
}

public sealed record May : MonthType
{
    public override string StartsWithValue { get; } = "ма";
    public override string DisplayName { get; } = "Май";
    public override int MonthNumber { get; } = 5;
}

public sealed record June : MonthType
{
    public override string StartsWithValue { get; } = "июн";
    public override string DisplayName { get; } = "Июнь";
    public override int MonthNumber { get; } = 6;
}

public sealed record July : MonthType
{
    public override string StartsWithValue { get; } = "июл";
    public override string DisplayName { get; } = "Июль";
    public override int MonthNumber { get; } = 7;
}

public sealed record August : MonthType
{
    public override string StartsWithValue { get; } = "август";
    public override string DisplayName { get; } = "Август";
    public override int MonthNumber { get; } = 8;
}

public sealed record September : MonthType
{
    public override string StartsWithValue { get; } = "сентябр";
    public override string DisplayName { get; } = "Сентябрь";
    public override int MonthNumber { get; } = 9;
}

public sealed record October : MonthType
{
    public override string StartsWithValue { get; } = "октябр";
    public override string DisplayName { get; } = "Октябрь";
    public override int MonthNumber { get; } = 10;
}

public sealed record November : MonthType
{
    public override string StartsWithValue { get; } = "ноябр";
    public override string DisplayName { get; } = "Ноябрь";
    public override int MonthNumber { get; } = 11;
}

public sealed record December : MonthType
{
    public override string StartsWithValue { get; } = "декабр";
    public override string DisplayName { get; } = "Декабрь";
    public override int MonthNumber { get; } = 12;
}

public static class MonthGenerator
{
    private static MonthType[] Months =>
        [
            new January(),
            new February(),
            new March(),
            new April(),
            new May(),
            new June(),
            new July(),
            new August(),
            new September(),
            new October(),
            new November(),
            new December(),
        ];

    public static Option<MonthType> TryGenerateFromInput(string input)
    {
        foreach (var month in Months)
        {
            if (input.StartsWith(month.StartsWithValue, StringComparison.OrdinalIgnoreCase))
                return Option<MonthType>.Some(month);
        }
        return Option<MonthType>.None();
    }

    public static int GetMonthNumber(this MonthType month)
    {
        return month switch
        {
            January j => j.MonthNumber,
            February f => f.MonthNumber,
            March m => m.MonthNumber,
            April ap => ap.MonthNumber,
            May m => m.MonthNumber,
            June j => j.MonthNumber,
            August au => au.MonthNumber,
            September sep => sep.MonthNumber,
            October october => october.MonthNumber,
            November november => november.MonthNumber,
            December december => december.MonthNumber,
            _ => throw new ArgumentException("Unknown detected month type"),
        };
    }

    public static DateTime GetDateTime(this MonthType month, int hours, int minutes, int monthDay)
    {
        int monthNumber = month.GetMonthNumber();
        int currentMonth = DateTime.Now.Month;

        int year = (monthNumber < currentMonth) ? DateTime.Now.Year + 1 : DateTime.Now.Year;
        DateTime date = new DateTime(year, monthNumber, monthDay)
            .AddHours(hours)
            .AddMinutes(minutes);
        return date;
    }
}
