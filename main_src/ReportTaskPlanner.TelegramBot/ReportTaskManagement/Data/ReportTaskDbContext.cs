using LiteDB.Async;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

public sealed class ReportTaskDbContext
{
    private readonly LiteDatabaseAsync _db = new(ReportTaskRepositoryConstants.ConnectionString);

    public ILiteCollectionAsync<ReportTask> Collection { get; }

    public ReportTaskDbContext() => Collection = _db.GetCollection<ReportTask>();
}
