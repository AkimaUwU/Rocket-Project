using LiteDB;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public sealed class ReportTaskRepositoryConstants
{
    public const string CollectionName = "Report_Tasks";
    public static readonly string ConnectionString = "Filename=ReportTasks.db;Connection=Shared";

    public static void RegisterMapping()
    {
        BsonMapper
            .Global.Entity<ReportTaskSchedule>()
            .Field(s => s.TimeToNotify, "time_to_notify")
            .Field(s => s.TimeCreated, "time_created")
            .Field(s => s.Remained, "remained");

        BsonMapper
            .Global.Entity<ReportTask>()
            .Id(x => x.Id, autoId: false)
            .Field(x => x.Text, "task_message")
            .Field(x => x.Schedule, "task_schedule");
    }
}
