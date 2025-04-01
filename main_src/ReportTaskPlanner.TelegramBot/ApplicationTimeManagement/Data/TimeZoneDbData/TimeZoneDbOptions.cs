namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;

public sealed class TimeZoneDbOptions
{
    public string Token { get; private set; } = string.Empty;

    private TimeZoneDbOptions() { } // lite db constructor.

    public TimeZoneDbOptions(string token) => Token = token;
}
