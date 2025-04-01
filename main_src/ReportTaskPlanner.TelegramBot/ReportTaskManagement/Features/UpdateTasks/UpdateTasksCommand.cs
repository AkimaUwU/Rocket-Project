using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks;

public sealed record UpdateTasksCommand(ReportTask[] Tasks) : ICommand<int>;

public sealed class UpdateTasksCommandHandler(IReportTaskRepository repository)
    : ICommandHandler<UpdateTasksCommand, int>
{
    private readonly IReportTaskRepository _repository = repository;

    public async Task<Result<int>> Handle(UpdateTasksCommand command)
    {
        ReportTask[] tasks = command.Tasks;
        int toUpdateCount = tasks.Length;
        int updatedCount = await _repository.UpdateMany(tasks);
        if (toUpdateCount != updatedCount)
            return new Error("Не все записи задач удалось обновить.");
        return updatedCount;
    }
}
