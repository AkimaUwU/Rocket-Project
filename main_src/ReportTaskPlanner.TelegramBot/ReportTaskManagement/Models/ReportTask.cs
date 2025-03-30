using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

public sealed class ReportTask
{
    public long Id { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public ReportTaskSchedule Schedule { get; private set; } = new ReportTaskSchedule(0, 0);

    private ReportTask() { } // lite db db constructor

    public ReportTask(long id, string text, ReportTaskSchedule schedule)
    {
        Id = id;
        Text = text;
        Schedule = schedule;
    }

    public override string ToString()
    {
        DateTime whenToFire = Schedule.TimeToNotify.FromUnixTime();
        return $"""
            {Text}

            Вызов задачи в: {whenToFire:HH:mm dd.MM.yyyy}
            """;
    }
}
