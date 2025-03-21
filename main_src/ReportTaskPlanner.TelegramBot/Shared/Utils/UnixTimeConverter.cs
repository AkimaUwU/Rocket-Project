namespace ReportTaskPlanner.TelegramBot.Shared.Utils;

public static class UnixTimeConverter
{
    public static long ToUnixTime(this DateTime dateTime)
    {
        DateTime utcDateTime = dateTime.ToUniversalTime();
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(utcDateTime - unixEpoch).TotalSeconds;
    }

    public static DateTime FromUnixTime(this long unixSeconds)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime utcDateTime = unixEpoch.AddSeconds(unixSeconds);
        return utcDateTime.ToLocalTime();
    }
}
