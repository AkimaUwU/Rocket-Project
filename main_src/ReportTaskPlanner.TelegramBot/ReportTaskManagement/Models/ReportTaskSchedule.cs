namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

public class ReportTaskSchedule
{
    public long TimeCreated { get; private set; }
    public long TimeToNotify { get; private set; }
    public long Remained => TimeToNotify - TimeCreated;

    private ReportTaskSchedule() { } // lite db constructor

    public ReportTaskSchedule(long timeCreated, long timeToNotify) =>
        (TimeCreated, TimeToNotify) = (timeCreated, timeToNotify);
}
