using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data.Decorators;

public sealed class ReportTaskRepositoryLogging(
    IReportTaskRepository repository,
    Serilog.ILogger logger
) : IReportTaskRepository
{
    private readonly IReportTaskRepository _repository = repository;
    private readonly Serilog.ILogger _logger = logger;
    private const string ErrorTemplate = "{Context} {Action} {Error}";
    private const string InfoTemplate = "{Context} {Action} {Info}";

    public async Task<Result> Save(ReportTask task)
    {
        Result result = await _repository.Save(task);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Error(
                    ErrorTemplate,
                    nameof(IReportTaskRepository),
                    nameof(Save),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    InfoTemplate,
                    nameof(IReportTaskRepository),
                    nameof(Save),
                    task.ToString()
                )
            )
            .BackToResult();
    }

    public async Task<ReportTask[]> GetPendingTasks(ApplicationTime time)
    {
        ReportTask[] tasks = await _repository.GetPendingTasks(time);
        _logger.Information(
            InfoTemplate,
            nameof(IReportTaskRepository),
            nameof(GetPendingTasks),
            tasks.Length
        );
        return tasks;
    }

    public async Task<Result<int>> RemoveMany(IEnumerable<ReportTask> tasks)
    {
        Result<int> result = await _repository.RemoveMany(tasks);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Information(
                    ErrorTemplate,
                    nameof(IReportTaskRepository),
                    nameof(RemoveMany),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    InfoTemplate,
                    nameof(IReportTaskRepository),
                    nameof(RemoveMany),
                    result.Value
                )
            )
            .BackToResult();
    }

    public async Task<Result<int>> UpdateMany(IEnumerable<ReportTask> tasks)
    {
        Result<int> result = await _repository.UpdateMany(tasks);
        return result
            .ToLoggerChunk(_logger)
            .LogOnError(logger =>
                logger.Information(
                    ErrorTemplate,
                    nameof(IReportTaskRepository),
                    nameof(UpdateMany),
                    result.Error.Message
                )
            )
            .LogOnSuccess(logger =>
                logger.Information(
                    InfoTemplate,
                    nameof(IReportTaskRepository),
                    nameof(UpdateMany),
                    result.Value
                )
            )
            .BackToResult();
    }
}
