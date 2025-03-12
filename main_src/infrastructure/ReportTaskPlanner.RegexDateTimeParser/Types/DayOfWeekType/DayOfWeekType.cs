namespace ReportTaskPlanner.RegexDateTimeParser.Types.DayOfWeekType;

public abstract record DayOfWeekType
{
    public abstract int Number { get; }
    public abstract string DisplayName { get; }
}

public sealed record Monday : DayOfWeekType
{
    public override int Number { get; } = 1;
    public override string DisplayName { get; } = "Понедельник";
}

public sealed record Tuesday : DayOfWeekType
{
    public override int Number { get; } = 2;
    public override string DisplayName { get; } = "Вторник";
}

public sealed record Wednesday : DayOfWeekType
{
    public override int Number { get; } = 3;
    public override string DisplayName { get; } = "Среда";
}

public sealed record Thursday : DayOfWeekType
{
    public override int Number { get; } = 4;
    public override string DisplayName { get; } = "Четверг";
}

public sealed record Friday : DayOfWeekType
{
    public override int Number { get; } = 5;
    public override string DisplayName { get; } = "Пятница";
}

public sealed record Saturday : DayOfWeekType
{
    public override int Number { get; } = 6;
    public override string DisplayName { get; } = "Суббота";
}

public sealed record Sunday : DayOfWeekType
{
    public override int Number { get; } = 7;
    public override string DisplayName { get; } = "Воскресенье";
}

public sealed record UnknownDayOfWeekType : DayOfWeekType
{
    public override int Number { get; } = -1;
    public override string DisplayName { get; } = "Unknown";
}

public static class DayOfWeekTypeGenerator
{
    public static DayOfWeekType GenerateFromCurrent() =>
        DateTime.Now.DayOfWeek switch
        {
            DayOfWeek.Monday => new Monday(),
            DayOfWeek.Tuesday => new Tuesday(),
            DayOfWeek.Wednesday => new Wednesday(),
            DayOfWeek.Thursday => new Thursday(),
            DayOfWeek.Friday => new Friday(),
            DayOfWeek.Saturday => new Saturday(),
            DayOfWeek.Sunday => new Sunday(),
            _ => throw new ArgumentException("Unknown day of week"),
        };

    public static DayOfWeekType GenerateFromInput(string input) =>
        input switch
        {
            null => new UnknownDayOfWeekType(),
            not null when string.IsNullOrWhiteSpace(input) => new UnknownDayOfWeekType(),
            not null when input.StartsWith("понедельн", StringComparison.OrdinalIgnoreCase) =>
                new Monday(),
            not null when input.StartsWith("вторн", StringComparison.OrdinalIgnoreCase) =>
                new Tuesday(),
            not null when input.StartsWith("сред", StringComparison.OrdinalIgnoreCase) =>
                new Wednesday(),
            not null when input.StartsWith("четверг", StringComparison.OrdinalIgnoreCase) =>
                new Thursday(),
            not null when input.StartsWith("пятниц", StringComparison.OrdinalIgnoreCase) =>
                new Friday(),
            not null when input.StartsWith("суббот", StringComparison.OrdinalIgnoreCase) =>
                new Saturday(),
            not null when input.StartsWith("воскресен", StringComparison.OrdinalIgnoreCase) =>
                new Sunday(),
            _ => new UnknownDayOfWeekType(),
        };
}
