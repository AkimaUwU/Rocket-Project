using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Utils;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

public sealed class ReportTask
{
    public long Id { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public ReportTaskSchedule Schedule { get; private set; } = new(0, 0);

    private ReportTask() { } // lite db db constructor

    public ReportTask(long id, string text, ReportTaskSchedule schedule)
    {
        Id = id;
        Text = text;
        Schedule = schedule;
    }

    public void AdjustNextPeriodic(ApplicationTime time)
    {
        if (!Schedule.IsPeriodic)
            return;
        long timeDifference = Schedule.TimeToNotify - Schedule.TimeCreated;
        long timeCreated = time.TimeStamp;
        long timeToNotify = time.TimeStamp + timeDifference;
        Schedule = new ReportTaskSchedule(Schedule, timeCreated, timeToNotify);
    }

    public override string ToString()
    {
        DateTime whenToFire = Schedule.TimeToNotify.FromUnixTime();
        return $"""
            {Text}

            Вызов задачи в: {whenToFire:HH:mm dd.MM.yyyy}

            Периодичность: {IsPeriodicString()}
            """;
    }

    private string IsPeriodicString() => Schedule.IsPeriodic ? "Да" : "Нет";
}

public static class ReportTaskExtensions
{
    public static void AdjustPeriodicTask(this ReportTask task, ApplicationTime time) =>
        task.AdjustNextPeriodic(time);

    public static void AdjustPeriodicTasks(this IEnumerable<ReportTask> tasks, ApplicationTime time)
    {
        foreach (ReportTask task in tasks)
            task.AdjustPeriodicTask(time);
    }

    public static IEnumerable<ReportTask> FilterBy(
        this IEnumerable<ReportTask> tasks,
        Func<ReportTask, bool> predicate
    ) => tasks.Where(predicate);
}
