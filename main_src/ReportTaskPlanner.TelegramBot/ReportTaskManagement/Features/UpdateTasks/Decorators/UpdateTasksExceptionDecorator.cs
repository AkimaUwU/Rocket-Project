using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks.Decorators;

public sealed class UpdateTasksExceptionDecorator(ICommandHandler<UpdateTasksCommand, int> handler)
    : ICommandHandler<UpdateTasksCommand, int>
{
    private readonly ICommandHandler<UpdateTasksCommand, int> _handler = handler;

    public async Task<Result<int>> Handle(UpdateTasksCommand command)
    {
        try
        {
            Result<int> updatedCount = await _handler.Handle(command);
            return updatedCount;
        }
        catch (Exception ex)
        {
            Error error = new Error($"Exception: {ex.Message}");
            return error;
        }
    }
}
