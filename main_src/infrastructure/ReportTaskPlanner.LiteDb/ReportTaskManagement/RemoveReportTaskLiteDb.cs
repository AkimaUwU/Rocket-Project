using LiteDB.Async;
using ReportTaskPlanner.LiteDb.Configurations;
using ReportTaskPlanner.UseCases.Extensions;
using ReportTaskPlanner.UseCases.ReportTaskManagement.DeleteReportTaskUseCase;
using ReportTaskPlanner.Utilities.ResultPattern;
using Serilog;

namespace ReportTaskPlanner.LiteDb.ReportTaskManagement;

[DbQuery(BaseType = typeof(RemoveReportTaskFromPersistance))]
public record RemoveReportTaskLiteDb : RemoveReportTaskFromPersistance
{
    protected readonly ILogger _logger;
    protected readonly LiteDbOptions _options;

    public RemoveReportTaskLiteDb(ILogger logger, LiteDbOptions options) =>
        (_logger, _options) = (logger, options);

    public override async Task<Result> Remove(DeleteReportTaskRequest request)
    {
        return (request.Id, request.Message) switch
        {
            (not null, null) => await new RemoveReportTaskUsingId(_logger, _options).Remove(
                request
            ),
            (null, not null) => await new RemoveReportTaskUsingMessage(_logger, _options).Remove(
                request
            ),
            _ => new Error(
                "Нельзя удалить задачу без параметра (ИД или сообщение)"
            ).LogErrorAndReturn(_logger, nameof(RemoveReportTaskLiteDb)),
        };
    }
}

internal sealed record RemoveReportTaskUsingId : RemoveReportTaskLiteDb
{
    public RemoveReportTaskUsingId(ILogger logger, LiteDbOptions options)
        : base(logger, options) { }

    public override async Task<Result> Remove(DeleteReportTaskRequest request)
    {
        using LiteDatabaseAsync db = new LiteDatabaseAsync(_options.ConnectionString);
        try
        {
            ILiteCollectionAsync<ReportTaskDao> collection = db.GetCollection<ReportTaskDao>(
                ReportTaskDao.Collection
            );

            int removed = await collection.DeleteManyAsync(task =>
                task.Id == Guid.Parse(request.Id)
            );
            if (removed > 0)
            {
                _logger.Information(
                    "{Context}. Удалена задача с ID: {Id}",
                    nameof(RemoveReportTaskUsingId),
                    request.Id
                );
                return Result.Success();
            }

            return new Error(
                $"Задача с ИД: {request.Id} не удалена. Не найдено."
            ).LogErrorAndReturn(_logger, nameof(RemoveReportTaskUsingId));
        }
        catch (Exception ex)
        {
            _logger.Fatal(
                "{Context}. Задача {Task} не удалена. Исключение: {Ex}",
                nameof(RemoveReportTaskUsingId),
                request,
                nameof(ex.Message)
            );
            return new Error($"Задача с ИД: {request.Id} не удалена. Исключение: {ex.Message}");
        }
    }
}

internal sealed record RemoveReportTaskUsingMessage : RemoveReportTaskLiteDb
{
    public RemoveReportTaskUsingMessage(ILogger logger, LiteDbOptions options)
        : base(logger, options) { }

    public override async Task<Result> Remove(DeleteReportTaskRequest request)
    {
        using LiteDatabaseAsync db = new LiteDatabaseAsync(_options.ConnectionString);
        try
        {
            ILiteCollectionAsync<ReportTaskDao> collection = db.GetCollection<ReportTaskDao>(
                ReportTaskDao.Collection
            );

            int removed = await collection.DeleteManyAsync(task => task.Message == request.Message);
            if (removed > 0)
            {
                _logger.Information(
                    "{Context}. Удалена задача с сообщением: {Message}",
                    nameof(RemoveReportTaskUsingMessage),
                    request.Message
                );
                return Result.Success();
            }

            return new Error(
                $"Задача с сообщением: {request.Message} не удалена. Не найдено."
            ).LogErrorAndReturn(_logger, nameof(RemoveReportTaskUsingMessage));
        }
        catch (Exception ex)
        {
            _logger.Fatal(
                "{Context}. Задача {Task} не удалена. Исключение: {Ex}",
                nameof(RemoveReportTaskUsingId),
                request,
                nameof(ex.Message)
            );
            return new Error(
                $"Задача с сообщением {request.Message} не удалена. Исключение: {ex.Message}"
            );
        }
    }
}
