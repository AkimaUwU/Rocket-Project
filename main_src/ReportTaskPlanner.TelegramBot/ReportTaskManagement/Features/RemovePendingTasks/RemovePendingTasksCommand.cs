using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;

public sealed record RemovePendingTasksCommand(ReportTask[] Tasks) : ICommand<int>;

public sealed class RemovePendingTasksCommandHandler(IReportTaskRepository repository)
    : ICommandHandler<RemovePendingTasksCommand, int>
{
    private readonly IReportTaskRepository _repository = repository;

    public async Task<Result<int>> Handle(RemovePendingTasksCommand command)
    {
        ReportTask[] tasks = command.Tasks;
        int toRemoveLength = tasks.Length;
        int removed = await _repository.RemoveMany(tasks);
        if (toRemoveLength != removed)
            return new Error(
                "Количество удаленных задач не = количеству удаленных задач при запросе"
            );
        return removed;
    }
}
