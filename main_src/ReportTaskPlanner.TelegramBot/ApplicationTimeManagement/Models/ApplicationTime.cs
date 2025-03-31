using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

public sealed class ApplicationTime
{
    public string ZoneName { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public long TimeStamp { get; private set; }
    public DateTime DateTime { get; private set; }
    public bool IsPeriodic { get; private set; }

    private ApplicationTime() { } // lite db constructor

    public ApplicationTime(
        string zoneName,
        string displayName,
        long timeStamp,
        DateTime dateTime,
        bool isPeriodic = false
    ) =>
        (ZoneName, DisplayName, TimeStamp, DateTime, IsPeriodic) = (
            zoneName,
            displayName,
            timeStamp,
            dateTime,
            isPeriodic
        );

    public ApplicationTime(ApplicationTime other)
        : this(other.ZoneName, other.DisplayName, other.TimeStamp, other.TimeStamp.FromUnixTime())
    { }

    public ApplicationTime(ApplicationTime other, bool isPeriodic)
        : this(other) => IsPeriodic = isPeriodic;

    public ApplicationTime(ApplicationTime other, long seconds)
        : this(other)
    {
        TimeStamp += seconds;
        DateTime = TimeStamp.FromUnixTime();
    }

    public ApplicationTime(ApplicationTime other, DateTime calculated)
        : this(other) => (TimeStamp, DateTime) = (calculated.ToUnixTime(), calculated);

    public override string ToString()
    {
        return $"""
            Информация о времени приложения:
            Тайм зона: {ZoneName}
            Время: {DateString}
            """;
    }

    public string DateString => DateTime.ToString("HH:mm:ss dd/MM/yyyy");
}
