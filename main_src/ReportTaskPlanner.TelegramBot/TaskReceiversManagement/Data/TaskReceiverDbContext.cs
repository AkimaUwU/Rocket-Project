using LiteDB.Async;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

public sealed class TaskReceiverDbContext
{
    private readonly LiteDatabaseAsync _db = new(TaskReceiverRepositoryConstants.ConnectionString);
    public ILiteCollectionAsync<TaskReceiver> Collection { get; }

    public TaskReceiverDbContext() =>
        Collection = _db.GetCollection<TaskReceiver>(
            TaskReceiverRepositoryConstants.CollectionName
        );
}
