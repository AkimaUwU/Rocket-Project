using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks.Decorators;

public sealed class RemovePendingTasksLoggingDecorator(
    ICommandHandler<RemovePendingTasksCommand, int> handler,
    Serilog.ILogger logger
) : ICommandHandler<RemovePendingTasksCommand, int>
{
    private readonly ICommandHandler<RemovePendingTasksCommand, int> _handler = handler;
    private readonly Serilog.ILogger _logger = logger;

    public async Task<Result<int>> Handle(RemovePendingTasksCommand command)
    {
        Result<int> removed = await _handler.Handle(command);
        return removed
            .ToLoggerChunk(_logger)
            .LogOnSuccess(logger =>
                logger.Information(
                    "{Context} removed {Count} tasks",
                    nameof(RemovePendingTasksCommand),
                    removed.Value
                )
            )
            .LogOnError(logger =>
                logger.Error(
                    "{Context} {Error}",
                    nameof(RemovePendingTasksCommand),
                    removed.Error.Message
                )
            )
            .BackToResult();
    }
}
