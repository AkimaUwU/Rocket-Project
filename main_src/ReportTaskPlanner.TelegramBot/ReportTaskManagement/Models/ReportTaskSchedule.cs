using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

public class ReportTaskSchedule
{
    public long TimeCreated { get; private set; }
    public long TimeToNotify { get; private set; }
    public bool IsPeriodic { get; private set; }

    public long Remained => TimeToNotify - TimeCreated;
    public DateTime DateTimeCreated => TimeCreated.FromUnixTime();
    public DateTime DateTimeToNotify => TimeToNotify.FromUnixTime();

    private ReportTaskSchedule() { } // ef core constructor

    public ReportTaskSchedule(long timeCreated, long timeToNotify) =>
        (TimeCreated, TimeToNotify) = (timeCreated, timeToNotify);

    public ReportTaskSchedule(long timeCreated, long timeToNotify, bool isPeriodic) =>
        (TimeCreated, TimeToNotify, IsPeriodic) = (timeCreated, timeToNotify, isPeriodic);

    public ReportTaskSchedule(ReportTaskSchedule other, long timeCreated, long timeToNotify)
        : this(timeCreated, timeToNotify) => IsPeriodic = other.IsPeriodic;

    public ReportTaskSchedule(ReportTaskSchedule other, bool isPeriodic)
        : this(other.TimeCreated, other.TimeToNotify) => IsPeriodic = isPeriodic;

    public override string ToString() =>
        $"""
            Task schedule info:
            *********************************************
            Unix created: {TimeCreated}
            Unix remained: {Remained}
            Unix notify: {TimeToNotify}
            *********************************************
            DateTime created: {DateString(DateTimeCreated)}
            DateTime notify: {DateString(DateTimeToNotify)}
            *********************************************
            Is periodic: {IsPeriodic}
            """;

    private static string DateString(DateTime dateTime) => dateTime.ToString("HH:mm:ss dd/MM/yyyy");
}
