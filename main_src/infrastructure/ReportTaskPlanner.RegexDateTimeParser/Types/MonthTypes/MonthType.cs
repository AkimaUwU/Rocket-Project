namespace ReportTaskPlanner.RegexDateTimeParser.Types.MonthTypes;

public abstract record MonthType
{
    public static DetectedMonthType[] Months =>
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
}

public sealed record UnknownMonthType : MonthType;

public abstract record DetectedMonthType : MonthType
{
    public abstract string StartsWithValue { get; }
    public abstract string DisplayName { get; }
    public abstract int MonthNumber { get; }
}

public sealed record January : DetectedMonthType
{
    public override string StartsWithValue { get; } = "январ";
    public override string DisplayName { get; } = "Январь";
    public override int MonthNumber { get; } = 1;
}

public sealed record February : DetectedMonthType
{
    public override string StartsWithValue { get; } = "феврал";
    public override string DisplayName { get; } = "Февраль";
    public override int MonthNumber { get; } = 2;
}

public sealed record March : DetectedMonthType
{
    public override string StartsWithValue { get; } = "март";
    public override string DisplayName { get; } = "Март";
    public override int MonthNumber { get; } = 3;
}

public sealed record April : DetectedMonthType
{
    public override string StartsWithValue { get; } = "апрел";
    public override string DisplayName { get; } = "Апрель";
    public override int MonthNumber { get; } = 4;
}

public sealed record May : DetectedMonthType
{
    public override string StartsWithValue { get; } = "ма";
    public override string DisplayName { get; } = "Май";
    public override int MonthNumber { get; } = 5;
}

public sealed record June : DetectedMonthType
{
    public override string StartsWithValue { get; } = "июн";
    public override string DisplayName { get; } = "Июнь";
    public override int MonthNumber { get; } = 6;
}

public sealed record July : DetectedMonthType
{
    public override string StartsWithValue { get; } = "июл";
    public override string DisplayName { get; } = "Июль";
    public override int MonthNumber { get; } = 7;
}

public sealed record August : DetectedMonthType
{
    public override string StartsWithValue { get; } = "август";
    public override string DisplayName { get; } = "Август";
    public override int MonthNumber { get; } = 8;
}

public sealed record September : DetectedMonthType
{
    public override string StartsWithValue { get; } = "сентябр";
    public override string DisplayName { get; } = "Сентябрь";
    public override int MonthNumber { get; } = 9;
}

public sealed record October : DetectedMonthType
{
    public override string StartsWithValue { get; } = "октябр";
    public override string DisplayName { get; } = "Октябрь";
    public override int MonthNumber { get; } = 10;
}

public sealed record November : DetectedMonthType
{
    public override string StartsWithValue { get; } = "ноябр";
    public override string DisplayName { get; } = "Ноябрь";
    public override int MonthNumber { get; } = 11;
}

public sealed record December : DetectedMonthType
{
    public override string StartsWithValue { get; } = "декабр";
    public override string DisplayName { get; } = "Декабрь";
    public override int MonthNumber { get; } = 12;
}

public static class MonthTypeExtensions
{
    public static bool IsInputIsMonthType(this DetectedMonthType[] types, string input) =>
        types.Any(t => t.DisplayName.StartsWith(input, StringComparison.OrdinalIgnoreCase));

    public static MonthType GetMatchedMonthType(this DetectedMonthType[] types, string input)
    {
        MonthType? month = types.FirstOrDefault(t =>
            input.StartsWith(t.StartsWithValue, StringComparison.OrdinalIgnoreCase)
        );
        return month == null ? new UnknownMonthType() : month;
    }

    public static int GetMonthNumber(this MonthType month)
    {
        if (month is UnknownMonthType)
            throw new ArgumentException("Unknown month type");
        if (month is DetectedMonthType detected)
        {
            return detected switch
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
        throw new ArgumentException("Unknown month type");
    }
}
