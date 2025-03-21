using LiteDB.Async;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;

public sealed class TimeZoneDbContext
{
    private readonly ILiteDatabaseAsync _db = new LiteDatabaseAsync(
        TimeZoneDbRepositoryConstants.ConnectionString
    );
    public ILiteCollectionAsync<TimeZoneDbOptions> Collection { get; }

    public TimeZoneDbContext() =>
        Collection = _db.GetCollection<TimeZoneDbOptions>(
            TimeZoneDbRepositoryConstants.CollectionName
        );
}
