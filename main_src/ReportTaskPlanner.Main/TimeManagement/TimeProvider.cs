using ReportTaskPlanner.Utilities.Common;

namespace ReportTaskPlanner.Main.TimeManagement;

public sealed record PlannerTime
{
    public string ZoneName { get; }
    public string DisplayName { get; }
    public ulong TimeStamp { get; }
    public DateTime DateTime { get; }
    
    public PlannerTime(string zoneName, string displayName, ulong timeStamp)
    {
        ZoneName = zoneName;
        DisplayName = displayName;
        TimeStamp = timeStamp;
        DateTime = FromUnixTimeStamp(timeStamp);
    }
    
    public UnixDateSeconds ToUnixDateSeconds() => new UnixDateSeconds((long)TimeStamp);

    private DateTime FromUnixTimeStamp(ulong timeStamp) =>
        new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeStamp);
}