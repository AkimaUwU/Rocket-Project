namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

public sealed class ApplicationTime
{
    public string ZoneName { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public long TimeStamp { get; private set; }
    public DateTime DateTime { get; private set; }

    private ApplicationTime() { } // lite db constructor

    public ApplicationTime(string zoneName, string displayName, long timeStamp, DateTime dateTime)
    {
        ZoneName = zoneName;
        DisplayName = displayName;
        TimeStamp = timeStamp;
        DateTime = dateTime;
    }

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
