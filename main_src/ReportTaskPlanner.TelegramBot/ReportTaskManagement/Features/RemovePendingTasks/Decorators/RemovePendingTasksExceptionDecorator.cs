using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks.Decorators;

public sealed class RemovePendingTasksExceptionDecorator(
    ICommandHandler<RemovePendingTasksCommand, int> handler
) : ICommandHandler<RemovePendingTasksCommand, int>
{
    private readonly ICommandHandler<RemovePendingTasksCommand, int> _handler = handler;

    public async Task<Result<int>> Handle(RemovePendingTasksCommand command)
    {
        try
        {
            Result<int> removedCount = await _handler.Handle(command);
            return removedCount;
        }
        catch (Exception ex)
        {
            return new Error($"Exception: {ex.Message}");
        }
    }
}
