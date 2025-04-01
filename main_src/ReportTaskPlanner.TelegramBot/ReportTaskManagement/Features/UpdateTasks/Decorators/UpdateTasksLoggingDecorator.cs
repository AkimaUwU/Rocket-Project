using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks.Decorators;

public sealed class UpdateTasksLoggingDecorator(
    ICommandHandler<UpdateTasksCommand, int> handler,
    Serilog.ILogger logger
) : ICommandHandler<UpdateTasksCommand, int>
{
    private readonly ICommandHandler<UpdateTasksCommand, int> _handler = handler;
    private readonly Serilog.ILogger _logger = logger;

    public async Task<Result<int>> Handle(UpdateTasksCommand command)
    {
        Result<int> updatedCount = await _handler.Handle(command);
        return updatedCount
            .ToLoggerChunk(_logger)
            .LogOnSuccess(logger =>
                logger.Information(
                    "{Context} updated {Count} tasks",
                    nameof(UpdateTasksCommand),
                    updatedCount.Value
                )
            )
            .LogOnError(logger =>
                logger.Error(
                    "{Context} {Error}",
                    nameof(UpdateTasksCommand),
                    updatedCount.Error.Message
                )
            )
            .BackToResult();
    }
}
