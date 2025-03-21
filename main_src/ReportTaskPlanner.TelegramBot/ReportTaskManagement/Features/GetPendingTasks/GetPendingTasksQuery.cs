using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.GetPendingTasks;

public sealed record GetPendingTasksQuery : IQuery<Option<IEnumerable<ReportTask>>>;

public sealed class GetPendingTasksQueryHandler(
    ReportTaskRepository tasksRepository,
    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> getAppTime
) : IQueryHandler<GetPendingTasksQuery, Option<IEnumerable<ReportTask>>>
{
    private readonly ReportTaskRepository _tasksRepository = tasksRepository;
    private readonly IQueryHandler<
        GetUpdatedApplicationTimeQuery,
        Option<ApplicationTime>
    > _getAppTime = getAppTime;

    public async Task<Option<IEnumerable<ReportTask>>> Handle(GetPendingTasksQuery query)
    {
        Option<ApplicationTime> appTime = await _getAppTime.Handle(new());
        if (!appTime.HasValue)
            return Option<IEnumerable<ReportTask>>.None();

        IEnumerable<ReportTask> tasks = await _tasksRepository.GetPendingTasks(appTime);
        return tasks.Any()
            ? Option<IEnumerable<ReportTask>>.Some(tasks)
            : Option<IEnumerable<ReportTask>>.None();
    }
}
