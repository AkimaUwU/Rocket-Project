using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;

public sealed record RemovePendingTasksCommand(ApplicationTime Time) : ICommand<bool>;

public sealed record RemovePendingTasksCommandHandler(ReportTaskRepository repository)
    : ICommandHandler<RemovePendingTasksCommand, bool>
{
    private readonly ReportTaskRepository _repository = repository;

    public async Task<Result<bool>> Handle(RemovePendingTasksCommand command)
    {
        await _repository.RemoveTasksByAppTime(command.Time);
        return true;
    }
}
