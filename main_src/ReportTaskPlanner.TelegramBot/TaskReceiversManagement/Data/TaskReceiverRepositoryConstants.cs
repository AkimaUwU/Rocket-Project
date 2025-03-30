using LiteDB;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public static class TaskReceiverRepositoryConstants
{
    public const string CollectionName = "Task_Receivers";
    public static readonly string ConnectionString = "Filename=TaskReceivers.db;Connection=Shared";

    public static void RegisterBsonMapping()
    {
        BsonMapper
            .Global.Entity<TaskReceiver>()
            .Id(r => r.Id, autoId: false)
            .Field(r => r.IsEnabled, "is_enabled");
    }
}
