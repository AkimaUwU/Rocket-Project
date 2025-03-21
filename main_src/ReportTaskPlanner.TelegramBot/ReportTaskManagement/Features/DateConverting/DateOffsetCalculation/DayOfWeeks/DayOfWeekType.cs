using System.Diagnostics;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DayOfWeeks;

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
    public override int Number { get; } = 0;
    public override string DisplayName { get; } = "Воскресенье";
}

public static class DayOfWeekTypeGenerator
{
    public static Option<DayOfWeekType> GenerateFromInput(string input) =>
        input switch
        {
            null => Option<DayOfWeekType>.None(),
            not null when string.IsNullOrWhiteSpace(input) => Option<DayOfWeekType>.None(),
            not null when input.StartsWith("понедель", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Monday()),
            not null when input.StartsWith("вторн", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Tuesday()),
            not null when input.StartsWith("сред", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Wednesday()),
            not null when input.StartsWith("четверг", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Thursday()),
            not null when input.StartsWith("пятниц", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Friday()),
            not null when input.StartsWith("суббот", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Saturday()),
            not null when input.StartsWith("воскресен", StringComparison.OrdinalIgnoreCase) =>
                Option<DayOfWeekType>.Some(new Sunday()),
            _ => Option<DayOfWeekType>.None(),
        };

    public static int GetDayNumber(this DayOfWeekType dayOfWeek)
    {
        return dayOfWeek switch
        {
            Monday m => m.Number,
            Tuesday t => t.Number,
            Wednesday w => w.Number,
            Thursday t => t.Number,
            Friday f => f.Number,
            Saturday s => s.Number,
            Sunday s => s.Number,
            _ => throw new UnreachableException("Day of week is not supported."),
        };
    }
}
