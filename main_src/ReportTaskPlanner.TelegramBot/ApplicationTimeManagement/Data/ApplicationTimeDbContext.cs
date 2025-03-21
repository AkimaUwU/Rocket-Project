using LiteDB.Async;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;

public sealed class ApplicationTimeDbContext
{
    private readonly LiteDatabaseAsync _db = new(
        ApplicationTimeRepositoryConstants.ConnectionString
    );
    public ILiteCollectionAsync<ApplicationTime> Collection { get; }

    public ApplicationTimeDbContext() =>
        Collection = _db.GetCollection<ApplicationTime>(
            ApplicationTimeRepositoryConstants.CollectionName
        );
}
