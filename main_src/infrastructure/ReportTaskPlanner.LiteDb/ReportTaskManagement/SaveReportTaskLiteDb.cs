using LiteDB;
using LiteDB.Async;
using ReportTaskPlanner.LiteDb.Configurations;
using ReportTaskPlanner.Main.ReportTasksManagement;
using ReportTaskPlanner.UseCases.CreateReportTaskUseCase;
using ReportTaskPlanner.Utilities.ResultPattern;
using Serilog;

namespace ReportTaskPlanner.LiteDb.ReportTaskManagement;

[DbQuery(BaseType = typeof(SaveReportTaskInPersistance))]
public sealed record SaveReportTaskLiteDb : SaveReportTaskInPersistance
{
    private readonly LiteDbOptions _options;
    private readonly ILogger _logger;

    public SaveReportTaskLiteDb(LiteDbOptions options, ILogger logger) =>
        (_options, _logger) = (options, logger);

    public override async Task<Result> Save(ReportTask reportTask, CancellationToken ct = default)
    {
        string connectionString = _options.ConnectionString;
        ReportTaskDao daoModel = new ReportTaskDao(reportTask);
        using LiteDatabaseAsync db = new LiteDatabaseAsync(connectionString);
        try
        {
            ILiteCollectionAsync<ReportTaskDao> collection = db.GetCollection<ReportTaskDao>(
                ReportTaskDao.Collection
            );
            await collection.InsertAsync(daoModel);
            _logger.Information(
                "{Context} saved Report Task {Task}",
                nameof(SaveReportTaskLiteDb),
                daoModel
            );
            await collection.EnsureIndexAsync(task => task.Id, unique: true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.Fatal("{Context} error {Exception}", nameof(SaveReportTaskLiteDb), ex.Message);
            return Result.Failure(ex.Message);
        }
    }
}
