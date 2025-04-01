using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.GetPendingTasks;

public sealed record GetPendingTasksQuery(ApplicationTime Time)
    : IQuery<Option<IEnumerable<ReportTask>>>;

public sealed class GetPendingTasksQueryHandler(IReportTaskRepository tasksRepository)
    : IQueryHandler<GetPendingTasksQuery, Option<IEnumerable<ReportTask>>>
{
    private readonly IReportTaskRepository _tasksRepository = tasksRepository;

    public async Task<Option<IEnumerable<ReportTask>>> Handle(GetPendingTasksQuery query)
    {
        IEnumerable<ReportTask> tasks = await _tasksRepository.GetPendingTasks(query.Time);
        return tasks.Any()
            ? Option<IEnumerable<ReportTask>>.Some(tasks)
            : Option<IEnumerable<ReportTask>>.None();
    }
}
